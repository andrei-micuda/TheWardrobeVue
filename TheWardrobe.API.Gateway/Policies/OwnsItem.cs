using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Flurl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using TheWardrobe.Helpers;

namespace TheWardrobe.API.Gateway.Policies
{
  public class OwnsItemRequirement : IAuthorizationRequirement { }

  public class OwnsItemHandler : AuthorizationHandler<OwnsItemRequirement>
  {
    private readonly HttpContext _httpContext;
    private readonly IDapperContext _dapperContext;

    public OwnsItemHandler(IHttpContextAccessor httpContextAccessor, IDapperContext dapperContext)
    {
      _httpContext = httpContextAccessor.HttpContext;
      _dapperContext = dapperContext;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnsItemRequirement requirement)
    {
      var itemId = new Url(_httpContext.Request.Path).PathSegments[3];

      var userId = context.User.Claims
          .Where(c => c.Type == "id")
          .Select(c => c.Value)
          .SingleOrDefault();

      using var connection = _dapperContext.GetConnection();
      var itemOwnerId = connection.ExecuteScalar<Guid>(@"
        SELECT seller_id
        FROM item
        WHERE id = @itemId", new { itemId = Guid.Parse(itemId) });

      if (userId == itemOwnerId.ToString())
        context.Succeed(requirement);
      return Task.CompletedTask;
    }
  }
}