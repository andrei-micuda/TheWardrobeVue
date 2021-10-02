using BCryptNet = BCrypt.Net.BCrypt;
using System.Collections.Generic;
using System.Linq;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Helpers;
using TheWardrobe.API.Models.Accounts;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;


namespace TheWardrobe.API.Repositories
{
  public interface IAccountService
  {
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    AuthenticateResponse RefreshToken(string token);
    void RevokeToken(string token);
    void Register(RegisterRequest model);
    // void VerifyEmail(string token);
    void ForgotPassword(ForgotPasswordRequest model);
    void ValidateResetToken(ValidateResetTokenRequest model);
    void ResetPassword(ResetPasswordRequest model);
    // IEnumerable<AccountResponse> GetAll();
    // AccountResponse GetById(int id);
    // AccountResponse Create(CreateRequest model);
    // AccountResponse Update(int id, UpdateRequest model);
    // void Delete(int id);
  }

  public class AccountService : IAccountService
  {
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly AppSettings _appSettings;

    public AccountService(IMapper mapper, IConfiguration configuration, IOptions<AppSettings> appSettings)
    {
      _mapper = mapper;
      _configuration = configuration;
      _appSettings = appSettings.Value;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
      using var connection = new NpgsqlConnection(_configuration["ConnectionString"]);
      var account = GetAccountByEmail(model.Email);

      account.RefreshTokens = connection.Query<RefreshToken>(@"
        SELECT *
        FROM refresh_tokens
        WHERE account_id = @account_id",
        new { account_id = account.Id }).ToList();

      if (account == null || !account.IsVerified || !BCryptNet.Verify(model.Password, account.PasswordHash))
        throw new AppException("Email or password is incorrect");


      // authentication successful, generate auth token and refresh token
      var response = _mapper.Map<AuthenticateResponse>(account);
      response.JwtToken = GenerateJwtToken(account);
      var refreshToken = GenerateRefreshToken();
      refreshToken.AccountId = account.Id;

      RemoveOldRefreshTokens(account);
      AddNewRefreshToken(refreshToken);

      response.RefreshToken = refreshToken.Token;
      return response;
    }

    public AuthenticateResponse RefreshToken(string token)
    {
      using var connection = new NpgsqlConnection(_configuration["ConnectionString"]);
      var (refreshToken, account) = GetRefreshToken(token);

      // replace old refresh token with a new one and save
      var newRefreshToken = GenerateRefreshToken();
      refreshToken.WhenRevoked = DateTime.UtcNow;
      refreshToken.ReplacedByToken = newRefreshToken.Token;

      connection.Execute(@"
        UPDATE refresh_tokens
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
      response.JwtToken = jwtToken;
      response.RefreshToken = newRefreshToken.Token;
      return response;
    }

    public void RevokeToken(string token)
    {
      using var connection = new NpgsqlConnection(_configuration["ConnectionString"]);

      var (refreshToken, account) = GetRefreshToken(token);

      // revoke token and save
      refreshToken.WhenRevoked = DateTime.UtcNow;

      connection.Execute(@"
        UPDATE refresh_tokens
        SET
          when_revoked = @WhenRevoked
        WHERE id = @Id
        ", refreshToken);
    }

    public void Register(RegisterRequest model)
    {
      // validate
      // if (_context.Accounts.Any(x => x.Email == model.Email))
      // {
      //   // send already registered error in email to prevent account enumeration
      //   sendAlreadyRegisteredEmail(model.Email, origin);
      //   return;
      // }

      // map model to new account object
      var account = _mapper.Map<Account>(model);

      account.WhenCreated = DateTime.UtcNow;
      account.VerificationToken = RandomTokenString();

      // hash password
      account.PasswordHash = BCryptNet.HashPassword(model.Password);

      using var connection = new NpgsqlConnection(_configuration["ConnectionString"]);

      connection.Execute(@"
        INSERT INTO accounts (first_name, last_name, email, password_hash, role, verification_token, when_verified, reset_token, when_password_reset, when_reset_token_expires, when_created, when_updated)
        VALUES (@FirstName, @LastName, @Email, @PasswordHash, @Role, @VerificationToken, @WhenVerified, @ResetToken, @WhenPasswordReset, @WhenResetTokenExpires, @WhenCreated, @WhenUpdated)",
        account);

      // send email
      // sendVerificationEmail(account, origin);
    }

    public void ForgotPassword(ForgotPasswordRequest model)
    {
      var account = GetAccountByEmail(model.Email);

      // always return ok response to prevent email enumeration
      if (account == null) return;

      // create reset token that expires after 1 day
      account.ResetToken = RandomTokenString();
      account.WhenResetTokenExpires = DateTime.UtcNow.AddDays(1);

      // update account fields
      using var connection = new NpgsqlConnection(_configuration["ConnectionString"]);
      connection.Execute(@"
      UPDATE accounts
      SET
        reset_token = @ResetToken,
        when_reset_token_expires = @WhenResetTokenExpires
      WHERE id = @Id", account);

      // send email
      // sendPasswordResetEmail(account, origin);
    }

    public void ValidateResetToken(ValidateResetTokenRequest model)
    {
      // var account = _context.Accounts.SingleOrDefault(x =>
      //     x.ResetToken == model.Token &&
      //     x.ResetTokenExpires > DateTime.UtcNow);
      using var connection = new NpgsqlConnection(_configuration["ConnectionString"]);
      var account = connection.QuerySingleOrDefault<Account>(@"
        SELECT *
        FROM accounts
        WHERE
          reset_token = @Token AND
          when_reset_token_expires > @UtcNow", new { Token = model.Token, UtcNow = DateTime.UtcNow });

      if (account == null)
        throw new AppException("Invalid token");
    }

    public void ResetPassword(ResetPasswordRequest model)
    {
      using var connection = new NpgsqlConnection(_configuration["ConnectionString"]);
      var account = connection.QuerySingleOrDefault<Account>(@"
        SELECT *
        FROM accounts
        WHERE
          reset_token = @Token AND
          when_reset_token_expires > @UtcNow", new { Token = model.Token, UtcNow = DateTime.UtcNow });

      if (account == null)
        throw new AppException("Invalid token");

      // update password and remove reset token
      account.PasswordHash = BCryptNet.HashPassword(model.Password);
      account.WhenPasswordReset = DateTime.UtcNow;
      account.ResetToken = null;
      account.WhenResetTokenExpires = null;

      connection.Execute(@"
      UPDATE accounts
      SET
        password_hash = @PasswordHash,
        when_password_reset = @UtcNow,
        reset_token = @ResetToken,
        when_reset_token_expires = @WhenResetTokenExpires
      WHERE id = @Id", new { account.PasswordHash, DateTime.UtcNow, account.ResetToken, account.WhenResetTokenExpires, account.Id });
    }

    private Account GetAccountByEmail(string email)
    {
      using var connection = new NpgsqlConnection(_configuration["ConnectionString"]);
      var account = connection.QuerySingleOrDefault<Account>(@"
        SELECT *
        FROM accounts
        WHERE email = @email",
        new { email });
      return account;
    }

    private string GenerateJwtToken(Account account)
    {
      // generate token that is valid for 7 days
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
        Expires = DateTime.UtcNow.AddMinutes(15),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    private (RefreshToken, Account) GetRefreshToken(string token)
    {
      using var connection = new NpgsqlConnection(_configuration["ConnectionString"]);
      var refreshToken = connection.QuerySingleOrDefault<RefreshToken>(@"
        SELECT * FROM refresh_tokens
        WHERE token = @Token", new { Token = token });

      if (refreshToken == null || !refreshToken.IsActive) throw new AppException("Invalid token");

      var account = connection.QuerySingleOrDefault<Account>(@"
        SELECT *
        FROM accounts
        WHERE id = @AccountId",
        new { AccountId = refreshToken.AccountId });

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
      using var connection = new NpgsqlConnection(_configuration["ConnectionString"]);

      // insert new refresh token into database
      connection.Execute(@"
        INSERT INTO refresh_tokens (account_id, token, when_expires, when_created, created_by_ip, when_revoked, revoked_by_ip, replaced_by_token)
        VALUES (@AccountId, @Token, @WhenExpires, @WhenCreated, @CreatedByIp, @WhenRevoked, @RevokedByIp, @ReplacedByToken)", refreshToken);
    }

    private void RemoveOldRefreshTokens(Account account)
    {
      using var connection = new NpgsqlConnection(_configuration["ConnectionString"]);

      var refreshTokensToDelete = account.RefreshTokens
          .Where(token =>
            !token.IsActive &&
            token.WhenCreated.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow)
          .Select(x => new { TokenId = x.Id })
          .ToArray();

      // save changes to Database
      connection.Execute(@"
        DELETE FROM refresh_tokens
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
  }
}