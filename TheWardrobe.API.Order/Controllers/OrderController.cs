using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWardrobe.API.Models;
using TheWardrobe.API.Repositories;
using TheWardrobe.CrossCutting.Helpers;

namespace TheWardrobe.API.Controllers
{
  [ApiController]
  [Route("/api/{accountId}/[controller]")]
  public class OrderController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<OrderController>();
    private readonly IOrderRepository _orderRepository;
    private readonly IAccountDetailsRepository _accountDetailsRepository;

    public OrderController(IOrderRepository orderRepository, IAccountDetailsRepository accountDetailsRepository)
    {
      _orderRepository = orderRepository;
      _accountDetailsRepository = accountDetailsRepository;
    }

    [HttpPost]
    public IActionResult PlaceOrder(Guid accountId, OrderRequest model)
    {
      _orderRepository.PlaceOrder(accountId, model);
      return Ok();
    }

    [HttpGet]
    public IActionResult GetOrders(Guid accountId, [FromQuery] OrderQueryFilters filters)
    {
      var res = _orderRepository.GetOrdersSummary(accountId, filters);
      foreach (var o in res.Orders)
      {
        o.Buyer = _accountDetailsRepository.GetAccountName(o.BuyerId);
        o.Seller = _accountDetailsRepository.GetAccountName(o.SellerId);
      }
      return Ok(res);
    }
  }
}
