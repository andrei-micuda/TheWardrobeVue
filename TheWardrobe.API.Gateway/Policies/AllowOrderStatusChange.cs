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
  public class AllowOrderStatusChangeRequirement : IAuthorizationRequirement { }

  public class AllowOrderStatusChangeHandler : AuthorizationHandler<AllowOrderStatusChangeRequirement>
  {
    private readonly HttpContext _httpContext;
    private readonly IDapperContext _dapperContext;

    public AllowOrderStatusChangeHandler(IHttpContextAccessor httpContextAccessor, IDapperContext dapperContext)
    {
      _httpContext = httpContextAccessor.HttpContext;
      _dapperContext = dapperContext;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowOrderStatusChangeRequirement requirement)
    {
      var orderIdStr = new Url(_httpContext.Request.Path).PathSegments[2];

      try
      {
        var orderId = Guid.Parse(orderIdStr);

        using var connection = _dapperContext.GetConnection();

        // Guid buyerId, sellerId;
        var (buyerId, sellerId) = connection.QueryFirstOrDefault<(Guid, Guid)>(@"
          SELECT buyer_id, seller_id
          FROM ""order""
          WHERE id = @orderId", new { orderId });

        var accountIdStr = context.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
        var accountId = Guid.Parse(accountIdStr);
        // context.User.Claims.Any( && c.Value == pathSellerId)

        if (accountId == buyerId || accountId == sellerId)
          context.Succeed(requirement);

        int x = 10;
      }
      catch (Exception ex)
      {

      }
      return Task.CompletedTask;
    }
  }
}