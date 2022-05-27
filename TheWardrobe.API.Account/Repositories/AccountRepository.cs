using BCryptNet = BCrypt.Net.BCrypt;
using System.Collections.Generic;
using System.Linq;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Helpers;
using TheWardrobe.API.Models.Accounts;
using Microsoft.Extensions.Configuration;
using Dapper;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using MassTransit;
using TheWardrobe.API.Publishers;
using System.Threading.Tasks;
using TheWardrobe.Helpers;
using Serilog;
using System.Data.Common;

namespace TheWardrobe.API.Repositories
{
  public interface IAccountRepository
  {
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    AuthenticateResponse RefreshToken(string token);
    void RevokeToken(string token, DbTransaction tx);
    Task Register(RegisterRequest model);
    void VerifyAccount(VerifyAccountRequest model);
    Task ForgotPassword(ForgotPasswordRequest model);
    void ChangePassword(ChangePasswordRequest model);
  }

  public class AccountRepository : IAccountRepository
  {
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;
    private readonly ISendEmailPublisher _sendEmailPublisher;
    private readonly IDapperContext _dapperContext;
    protected readonly ILogger _log = Log.ForContext<AccountRepository>();

    public AccountRepository(IMapper mapper, IDapperContext dapperContext, IOptions<AppSettings> appSettings, ISendEmailPublisher sendEmailPublisher)
    {
      _mapper = mapper;
      _dapperContext = dapperContext;
      _appSettings = appSettings.Value;
      _sendEmailPublisher = sendEmailPublisher;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
      using var connection = _dapperContext.GetConnection();
      var account = GetAccountByEmail(model.Email);

      if (account == null || !account.IsVerified || !BCryptNet.Verify(model.Password, account.PasswordHash))
        throw new AppException("Email or password is incorrect");

      account.RefreshTokens = connection.Query<RefreshToken>(@"
        SELECT *
        FROM refresh_token
        WHERE account_id = @account_id",
        new { account_id = account.Id }).ToList();

      // authentication successful, generate auth token and refresh token
      var response = _mapper.Map<AuthenticateResponse>(account);
      response.Jwt = GenerateJwtToken(account);
      var refreshToken = GenerateRefreshToken();
      refreshToken.AccountId = account.Id;

      RemoveOldRefreshTokens(account);
      AddNewRefreshToken(refreshToken);

      response.RefreshToken = refreshToken.Token;
      return response;
    }

    public AuthenticateResponse RefreshToken(string token)
    {
      using var connection = _dapperContext.GetConnection(); ;
      var (refreshToken, account) = GetRefreshToken(token);

      // replace old refresh token with a new one and save
      var newRefreshToken = GenerateRefreshToken();
      newRefreshToken.AccountId = account.Id;

      refreshToken.WhenRevoked = DateTime.UtcNow;
      refreshToken.ReplacedByToken = newRefreshToken.Token;

      connection.Execute(@"
        UPDATE refresh_token
        SET
          when_revoked = @WhenRevoked,
          replaced_by_token = @Token
        WHERE id = @Id
        ", refreshToken);
      AddNewRefreshToken(newRefreshToken);
      RemoveOldRefreshTokens(account);

      // generate new jwt
      var jwtToken = GenerateJwtToken(account);

      var response = _mapper.Map<AuthenticateResponse>(account);
      response.Jwt = jwtToken;
      response.RefreshToken = newRefreshToken.Token;
      return response;
    }

    public void RevokeToken(string token, DbTransaction tx = null)
    {
      using var connection = _dapperContext.GetConnection(); ;

      var (refreshToken, account) = GetRefreshToken(token);

      // revoke token and save
      refreshToken.WhenRevoked = DateTime.UtcNow;

      connection.Execute(@"
        UPDATE refresh_token
        SET
          when_revoked = @WhenRevoked
        WHERE id = @Id
        ", refreshToken, transaction: tx);
    }

    public async Task Register(RegisterRequest model)
    {
      using var connection = _dapperContext.GetConnection();
      var duplicateEmail = connection.ExecuteScalar<string>("SELECT email FROM account WHERE email = @email LIMIT 1", new
      {
        email = model.Email
      });

      if (!string.IsNullOrEmpty(duplicateEmail))
      {
        // TODO
        // send already registered error in email to prevent account enumeration
        // sendAlreadyRegisteredEmail(model.Email, origin);
        return;
      }

      // map model to new account object
      var account = _mapper.Map<Account>(model);

      account.WhenCreated = DateTime.UtcNow;
      account.WhenUpdated = DateTime.UtcNow;
      account.VerificationToken = RandomTokenString();

      // hash password
      account.PasswordHash = BCryptNet.HashPassword(model.Password);

      connection.Insert<Guid, Account>(account);

      await _sendEmailPublisher.SendVerificationEmail(account.Email, account.VerificationToken);
    }

    public async Task ForgotPassword(ForgotPasswordRequest model)
    {
      var account = GetAccountByEmail(model.Email);

      // always return ok response to prevent email enumeration
      if (account == null) return;

      // create reset token that expires after 1 day
      account.ResetToken = RandomTokenString();
      account.WhenResetTokenExpires = DateTime.UtcNow.AddDays(1);

      // update account fields
      using var connection = _dapperContext.GetConnection(); ;
      connection.Execute(@"
      UPDATE account
      SET
        reset_token = @ResetToken,
        when_reset_token_expires = @WhenResetTokenExpires
      WHERE id = @Id", account);

      // send email
      await _sendEmailPublisher.SendForgotPasswordEmail(account.Email, account.ResetToken);
    }

    public void ChangePassword(ChangePasswordRequest model)
    {
      using var connection = _dapperContext.GetConnection(); ;
      var account = connection.QuerySingleOrDefault<Account>(@"
        SELECT *
        FROM account
        WHERE
          reset_token = @Token AND
          when_reset_token_expires > now()", new { model.Token });

      if (account == null)
        throw new AppException("Invalid token");

      // work within a sql transaction in order to treat password reset & token revocation as a unit
      connection.Open();
      var tx = connection.BeginTransaction();

      // update password and remove reset token
      account.PasswordHash = BCryptNet.HashPassword(model.Password);
      account.WhenPasswordReset = DateTime.UtcNow;
      account.ResetToken = null;
      account.WhenResetTokenExpires = null;

      try
      {
        connection.Execute(@"
          UPDATE account
          SET
            password_hash = @PasswordHash,
            when_password_reset = now(),
            reset_token = @ResetToken,
            when_reset_token_expires = @WhenResetTokenExpires
          WHERE id = @Id", new { account.PasswordHash, account.ResetToken, account.WhenResetTokenExpires, account.Id },
          transaction: tx);

        // revoke all current refresh tokens
        var refreshTokensToRevoke = connection.Query<string>(@"
          SELECT token
          FROM refresh_token
          WHERE account_id = @Id
          AND when_revoked IS NULL", new { account.Id });

        foreach (var refreshToken in refreshTokensToRevoke)
        {
          RevokeToken(refreshToken, tx);
        }

        tx.Commit();
      }
      catch (Exception ex)
      {
        _log.Error(ex.Message);
        tx.Rollback();
      }
      finally
      {
        connection.Close();
      }

    }

    private Account GetAccountByEmail(string email)
    {
      using var connection = _dapperContext.GetConnection();
      var account = connection.QuerySingleOrDefault<Account>(@"
        SELECT *
        FROM account
        WHERE email = @email",
        new { email });

      return account;
    }

    private string GenerateJwtToken(Account account)
    {
      // generate token that is valid for 15 minutes
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
        Expires = DateTime.UtcNow.AddMinutes(_appSettings.RefreshTokenTTL),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    private (RefreshToken, Account) GetRefreshToken(string token)
    {
      using var connection = _dapperContext.GetConnection();
      var refreshToken = connection.QuerySingleOrDefault<RefreshToken>(@"
        SELECT * FROM refresh_token
        WHERE token = @Token", new { Token = token });

      if (refreshToken == null || !refreshToken.IsActive) throw new AppException("Invalid token");

      var account = connection.QuerySingleOrDefault<Account>(@"
        SELECT *
        FROM account
        WHERE id = @AccountId",
        new { refreshToken.AccountId });

      account.RefreshTokens = connection.Query<RefreshToken>(@"
        SELECT *
        FROM refresh_token
        WHERE account_id = @AccountId", new { refreshToken.AccountId }).ToList();

      return (refreshToken, account);
    }

    private static RefreshToken GenerateRefreshToken()
    {
      return new RefreshToken
      {
        Token = RandomTokenString(),
        WhenExpires = DateTime.UtcNow.AddDays(7),
        WhenCreated = DateTime.UtcNow,
      };
    }

    private void AddNewRefreshToken(RefreshToken refreshToken)
    {
      using var connection = _dapperContext.GetConnection();

      // insert new refresh token into database
      connection.Insert<Guid, RefreshToken>(refreshToken);
    }

    private void RemoveOldRefreshTokens(Account account)
    {
      using var connection = _dapperContext.GetConnection();

      var refreshTokensToDelete = account.RefreshTokens
          .Where(token =>
            !token.IsActive &&
            token.WhenCreated.AddMinutes(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow)
          .Select(x => new { TokenId = x.Id })
          .ToArray();

      // save changes to Database
      connection.Execute(@"
        DELETE FROM refresh_token
        WHERE id = @TokenId;",
        refreshTokensToDelete);
    }

    private static string RandomTokenString()
    {
      var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
      var randomBytes = new byte[40];
      rngCryptoServiceProvider.GetBytes(randomBytes);
      // convert random bytes to hex string
      return BitConverter.ToString(randomBytes).Replace("-", "");
    }

    public void VerifyAccount(VerifyAccountRequest model)
    {
      using var connection = _dapperContext.GetConnection();

      connection.Execute(@"
        UPDATE account
        SET when_verified = now()
        WHERE verification_token = @VerificationToken
      ", new { model.VerificationToken });
    }
  }
}