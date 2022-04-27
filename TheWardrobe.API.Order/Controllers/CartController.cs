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
  public class CartController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<CartController>();
    private readonly ICartRepository _cartRepository;
    private readonly IItemCatalogRepository _itemCatalogRepository;
    private readonly IAccountDetailsRepository _accountDetailsRepository;

    public CartController(ICartRepository cartRepository, IItemCatalogRepository itemCatalogRepository, IAccountDetailsRepository accountDetailsRepository)
    {
      _cartRepository = cartRepository;
      _itemCatalogRepository = itemCatalogRepository;
      _accountDetailsRepository = accountDetailsRepository;
    }

    [HttpGet]
    public IActionResult GetCart(Guid accountId, [FromQuery] Guid? sellerId)
    {
      var itemIds = _cartRepository.GetCart(accountId, sellerId);
      var items = itemIds.Select(id => _itemCatalogRepository.GetItem(id));

      var res = items.Select(i =>
      {
        i.Seller = _accountDetailsRepository.GetAccountName(i.SellerId);
        return i;
      });

      return Ok(res);
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
