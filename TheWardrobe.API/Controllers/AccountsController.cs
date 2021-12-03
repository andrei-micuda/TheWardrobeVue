using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Helpers;
using TheWardrobe.API.Models.Accounts;
using TheWardrobe.API.Repositories;

namespace TheWardrobe.API.Controllers
{

  [ApiController]
  [Route("[controller]")]
  public class AccountsController : BaseController
  {
    private readonly IMapper _mapper;
    private readonly IAccountRepository _accountService;

    public AccountsController(
            IMapper mapper,
            IAccountRepository accountService)
    {
      _mapper = mapper;
      _accountService = accountService;
    }

    [HttpGet("hello")]
    public IActionResult Hello()
    {
      return Ok("Hello world!");
    }

    [HttpPost("authenticate")]
    public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
      var response = _accountService.Authenticate(model);
      SetTokenCookie(response.RefreshToken);
      return Ok(response);
    }

    [HttpPost("refresh-token")]
    public ActionResult<AuthenticateResponse> RefreshToken()
    {
      var refreshToken = Request.Cookies["refreshToken"];
      var response = _accountService.RefreshToken(refreshToken);
      SetTokenCookie(response.RefreshToken);
      return Ok(response);
    }

    [Authorize]
    [HttpPost("revoke-token")]
    public IActionResult RevokeToken(RevokeTokenRequest model)
    {
      // accept token from request body or cookie
      var token = model.Token ?? Request.Cookies["refreshToken"];

      if (string.IsNullOrEmpty(token))
        return BadRequest(new { message = "Token is required" });

      // users can revoke their own tokens and admins can revoke any tokens
      if (!Account.OwnsToken(token) && Account.Role != Role.Admin)
        return Unauthorized(new { message = "Unauthorized" });

      _accountService.RevokeToken(token);
      return Ok(new { message = "Token revoked" });
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest model)
    {
      _accountService.Register(model);
      return Ok(new { message = "Registration successful, please check your email for verification instructions" });
    }

    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword(ForgotPasswordRequest model)
    {
      _accountService.ForgotPassword(model);
      return Ok(new { message = "Please check your email for password reset instructions" });
    }

    [HttpPost("validate-reset-token")]
    public IActionResult ValidateResetToken(ValidateResetTokenRequest model)
    {
      _accountService.ValidateResetToken(model);
      return Ok(new { message = "Token is valid" });
    }

    [HttpPost("reset-password")]
    public IActionResult ResetPassword(ResetPasswordRequest model)
    {
      _accountService.ResetPassword(model);
      return Ok(new { message = "Password reset successful, you can now login" });
    }

    [Authorize]
    [HttpGet("test")]
    public IActionResult Test()
    {
      return Ok(new { message = "Working!" });
    }

    // helper methods
    private void SetTokenCookie(string token)
    {
      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(7)
      };
      Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
  }
}