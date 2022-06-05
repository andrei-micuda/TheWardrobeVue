using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Interfaces;
using TheWardrobe.API.Models;
using TheWardrobe.API.Repositories;
using TheWardrobe.Helpers;

namespace TheWardrobe.API.Controllers
{
  [ApiController]
  [Route("/public/api/{accountId}/[controller]")]
  public class OrderController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<OrderController>();
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly AccountDetailsInterface _accountDetailsInterface;
    private readonly DeliveryAddressInterface _deliveryAddressInterface;
    private readonly ItemCatalogInterface _itemCatalogInterface;

    public OrderController(IConfiguration config, IOrderRepository orderRepository, ICartRepository cartRepository)
    {
      _orderRepository = orderRepository;
      _cartRepository = cartRepository;
      _accountDetailsInterface = new AccountDetailsInterface(config);
      _deliveryAddressInterface = new DeliveryAddressInterface(config);
      _itemCatalogInterface = new ItemCatalogInterface(config);
    }

    [HttpPost]
    public IActionResult PlaceOrder(Guid accountId, OrderRequest model)
    {
      _cartRepository.RemoveBulk(accountId, model.Items);
      _orderRepository.PlaceOrder(accountId, model);
      return Ok();
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderById(Guid accountId, Guid orderId)
    {
      var order = _orderRepository.GetOrder(accountId, orderId);
      order.Buyer = await _accountDetailsInterface.GetAccountName(order.BuyerId);
      order.Seller = await _accountDetailsInterface.GetAccountName(order.SellerId);
      order.DeliveryAddress = await _deliveryAddressInterface.GetDeliveryAddress(order.DeliveryAddress.Id);

      order.OrderItems = await Task.WhenAll(_orderRepository.GetOrderItemIds(orderId)
                                          .Select(itemId => _itemCatalogInterface.GetItem(itemId)));

      order.Total = order.OrderItems.Sum(item => item.Price);
      return Ok(order);
    }

    [HttpGet("ratings")]
    public IActionResult GetRatings(Guid accountId)
    {
      var buyerRating = _orderRepository.GetBuyerRating(accountId);
      var sellerRating = _orderRepository.GetSellerRating(accountId);

      return Ok(new { buyerRating, sellerRating });
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(Guid accountId, [FromQuery] OrderQueryFilters filters)
    {
      var res = _orderRepository.GetOrdersSummary(accountId, filters);
      foreach (var o in res.Orders)
      {
        o.Buyer = await _accountDetailsInterface.GetAccountName(o.BuyerId);
        o.Seller = await _accountDetailsInterface.GetAccountName(o.SellerId);
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
