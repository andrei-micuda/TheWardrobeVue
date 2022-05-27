using System;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Helpers;
using TheWardrobe.API.Models.Accounts;
using TheWardrobe.API.Repositories;
using TheWardrobe.CrossCutting;
using TheWardrobe.CrossCutting.Messages;

namespace TheWardrobe.API.Controllers
{

  [ApiController]
  [Route("/public/api/[controller]")]
  public class AccountController : BaseController
  {
    private readonly IMapper _mapper;
    private readonly IAccountRepository _accountService;
    private readonly IBus _bus;

    public AccountController(
            IMapper mapper,
            IAccountRepository accountService,
            IBus bus)
    {
      _mapper = mapper;
      _accountService = accountService;
      _bus = bus;
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

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest model)
    {
      await _accountService.Register(model);
      return Ok(new { message = "Registration successful, please check your email for verification instructions" });
    }

    [HttpPost("verify")]
    public IActionResult VerifyAccount(VerifyAccountRequest model)
    {
      _accountService.VerifyAccount(model);
      return Ok();
    }

    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword(ForgotPasswordRequest model)
    {
      _accountService.ForgotPassword(model);
      return Ok(new { message = "Please check your email for password reset instructions" });
    }

    [HttpPost("change-password")]
    public IActionResult ChangePassword(ChangePasswordRequest model)
    {
      _accountService.ChangePassword(model);
      return Ok(new { message = "Password changed successful, you can now login" });
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