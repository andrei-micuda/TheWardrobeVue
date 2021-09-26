using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using TheWardrobe.API.Helpers;
using TheWardrobe.API.Services;

namespace TheWardrobe.API.Authorization
{
  public class JwtMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
      _next = next;
      _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
      var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
      var userId = jwtUtils.ValidateToken(token);
      if (userId != null)
      {
        // attach user to context on successful jwt validation
        // context.Items["User"] = userService.GetById(userId.Value);
        context.Items["User"] = userId;
      }

      await _next(context);
    }
  }
}