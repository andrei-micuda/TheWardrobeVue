using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWardrobe.API.Entities;
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
    private readonly IDeliveryAddressRepository _deliveryAddressRepository;
    private readonly IItemCatalogRepository _itemCatalogRepository;

    public OrderController(IOrderRepository orderRepository, IAccountDetailsRepository accountDetailsRepository, IDeliveryAddressRepository deliveryAddressRepository, IItemCatalogRepository itemCatalogRepository)
    {
      _orderRepository = orderRepository;
      _accountDetailsRepository = accountDetailsRepository;
      _deliveryAddressRepository = deliveryAddressRepository;
      _itemCatalogRepository = itemCatalogRepository;
    }

    [HttpPost]
    public IActionResult PlaceOrder(Guid accountId, OrderRequest model)
    {
      _orderRepository.PlaceOrder(accountId, model);
      return Ok();
    }

    [HttpGet("{orderId}")]
    public IActionResult GetOrderById(Guid accountId, Guid orderId)
    {
      var order = _orderRepository.GetOrder(accountId, orderId);
      order.Buyer = _accountDetailsRepository.GetAccountName(order.BuyerId);
      order.Seller = _accountDetailsRepository.GetAccountName(order.SellerId);
      order.DeliveryAddress = _deliveryAddressRepository.Get(order.DeliveryAddress.Id);

      order.OrderItems = _orderRepository.GetOrderItemIds(orderId)
                                          .Select(itemId => _itemCatalogRepository.GetItem(itemId));

      order.Total = order.OrderItems.Sum(item => item.Price);
      return Ok(order);
    }

    [HttpGet("ratings")]
    public IActionResult GetOrders(Guid accountId)
    {
      var buyerRating = _orderRepository.GetBuyerRating(accountId);
      var sellerRating = _orderRepository.GetSellerRating(accountId);

      return Ok(new { buyerRating, sellerRating });
    }

    [HttpGet]
    public IActionResult GetRatings(Guid accountId, [FromQuery] OrderQueryFilters filters)
    {
      var res = _orderRepository.GetOrdersSummary(accountId, filters);
      foreach (var o in res.Orders)
      {
        o.Buyer = _accountDetailsRepository.GetAccountName(o.BuyerId);
        o.Seller = _accountDetailsRepository.GetAccountName(o.SellerId);
      }
      return Ok(res);
    }

    [HttpPatch("{orderId}")]
    public IActionResult UpdateOrderStatus(Guid accountId, Guid orderId, [FromBody] OrderStatusChangeRequest model)
    {
      var wasUpdated = _orderRepository.UpdateOrderStatus(accountId, orderId, model.Status);

      if (wasUpdated)
        return Ok();
      return BadRequest();
    }

    [HttpPatch("{orderId}/review")]
    public IActionResult ReviewOrder(Guid accountId, Guid orderId, [FromBody] ReviewRequest model)
    {
      var wasSuccessful = _orderRepository.ReviewOrder(accountId, orderId, model.Rating);

      if (wasSuccessful)
        return Ok();
      return BadRequest();
    }
  }
}
