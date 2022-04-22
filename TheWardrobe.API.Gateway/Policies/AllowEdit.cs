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

    public AllowEditHandler(IHttpContextAccessor httpContextAccessor)
    {
      _httpContext = httpContextAccessor.HttpContext;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowEditRequirement requirement)
    {
      var pathSellerId = new Url(_httpContext.Request.Path).PathSegments[1];

      if (context.User.Claims.Any(c => c.Type == "id" && c.Value == pathSellerId))
        context.Succeed(requirement);
      return Task.CompletedTask;
    }
  }
}