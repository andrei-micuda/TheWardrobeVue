using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace TheWardrobe.API.Gateway.Policies
{
  public class AllowEditRequirement : IAuthorizationRequirement { }

  public class AllowEditHandler : AuthorizationHandler<AllowEditRequirement>
  {
    private readonly HttpContext _httpContext;
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<AllowEditHandler>();

    public AllowEditHandler(IHttpContextAccessor httpContextAccessor)
    {
      _httpContext = httpContextAccessor.HttpContext;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowEditRequirement requirement)
    {
      var pathAccountId = new Url(_httpContext.Request.Path).PathSegments[2];

      if (context.User.Claims.Any(c => c.Type == "id" && c.Value == pathAccountId))
      {
        context.Succeed(requirement);
        _log.Information("AllowEdit policy succeeded for {accountId}.", pathAccountId);
      }
      else
      {
        _log.Information("AllowEdit policy FAILED for {accountId}.", pathAccountId);
      }
      return Task.CompletedTask;
    }
  }
}