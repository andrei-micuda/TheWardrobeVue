using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

    public OrderController(IOrderRepository orderRepository)
    {
      _orderRepository = orderRepository;
    }

    [HttpGet]
    public IActionResult GetOrders(Guid accountId)
    {
      var res = _orderRepository.GetOrders(accountId);
      return Ok(res);
    }
  }
}
