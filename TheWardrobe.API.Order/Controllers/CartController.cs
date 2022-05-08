using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TheWardrobe.API.Interfaces;
using TheWardrobe.API.Models;
using TheWardrobe.API.Repositories;
using TheWardrobe.Helpers;

namespace TheWardrobe.API.Controllers
{
  [ApiController]
  [Route("/public/api/{accountId}/[controller]")]
  public class CartController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<CartController>();
    private readonly ICartRepository _cartRepository;
    private readonly AccountDetailsInterface _accountDetailsInterface;
    private readonly ItemCatalogInterface _itemCatalogInterface;

    public CartController(IConfiguration config, ICartRepository cartRepository)
    {
      _cartRepository = cartRepository;
      _accountDetailsInterface = new AccountDetailsInterface(config);
      _itemCatalogInterface = new ItemCatalogInterface(config);
    }

    [HttpGet]
    [Route("{itemId}")]
    public IActionResult GetFavoriteStatus(Guid accountId, Guid itemId)
    {
      var isInCart = _cartRepository.CheckIsInCart(accountId, itemId);
      return Ok(new { isInCart });
    }

    [HttpGet]
    public async Task<IActionResult> GetCart(Guid accountId, [FromQuery] Guid? sellerId)
    {
      var itemIds = _cartRepository.GetCart(accountId, sellerId);

      var items = await Task.WhenAll(
        itemIds.Select(async id => await _itemCatalogInterface.GetItem(id)));

      foreach (var item in items)
      {
        item.Seller = await _accountDetailsInterface.GetAccountName(item.SellerId);
      }

      return Ok(items);
    }

    [HttpPost]
    public IActionResult Post(Guid accountId, CartRequest model)
    {
      _cartRepository.Add(accountId, model);
      return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(Guid accountId, CartRequest model)
    {
      _cartRepository.Remove(accountId, model);
      return Ok();
    }
  }
}
