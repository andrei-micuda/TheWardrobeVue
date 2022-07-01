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

namespace TheWardrobe.API.ItemCatalog.Controllers
{
  [ApiController]
  // [Route("/public/api/[controller]/[action]")]
  public class ItemCatalogController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<ItemCatalogController>();
    private readonly IItemCatalogRepository _itemCatalogRepository;
    private readonly AccountDetailsInterface _accountDetailsInterface;

    public ItemCatalogController(IConfiguration config, IItemCatalogRepository itemCatalogRepository)
    {
      _itemCatalogRepository = itemCatalogRepository;
      _accountDetailsInterface = new AccountDetailsInterface(config);
    }

    [HttpGet]
    [Route("/public/api/[controller]/{userId}/ids")]
    public IActionResult GetAllItemIds(Guid userId)
    {
      var itemIds = _itemCatalogRepository.GetAllItemIds(userId);
      return Ok(itemIds);
    }

    [HttpGet]
    [Route("/public/api/[controller]")]
    public IActionResult GetItems([FromQuery] ItemQueryFilters filters)
    {
      // sanity check query parameters
      if (filters.SellerIdExclude != null && filters.SellerIdInclude != null)
      {
        // if both are provided return BadRequest
        return BadRequest("Please only provide one of the following query parameters: sellerIdInclude, sellerIdExclude");
      }

      ItemListResponse items;

      try
      {
        items = _itemCatalogRepository.GetItems(filters);
      }
      catch (AppException ex)
      {
        return BadRequest(ex.Message);
      }
      return Ok(items);
    }

    [HttpPost]
    [Route("/public/api/[controller]")]
    public IActionResult Post(ItemRequestResponse model)
    {
      var item = _itemCatalogRepository.InsertItem(model);
      return Ok(item);
    }

    [HttpGet]
    [Route("/public/api/[controller]/{itemId}")]
    public async Task<IActionResult> GetItemById(Guid itemId)
    {
      var item = _itemCatalogRepository.GetItem(itemId);
      item.Seller = await _accountDetailsInterface.GetAccountName(item.SellerId);
      return Ok(item);
    }


    [HttpPut]
    [Route("/public/api/[controller]/{itemId}")]
    public IActionResult UpdateItemById(Guid itemId, ItemRequestResponse model)
    {
      model.Id = itemId;
      _itemCatalogRepository.UpdateItem(model);
      return Ok();
    }

    [HttpDelete]
    [Route("/public/api/[controller]/{itemId}")]
    public IActionResult DeleteItemById(Guid itemId)
    {
      _itemCatalogRepository.DeleteItem(itemId);
      return Ok();
    }

    [HttpPatch]
    [Route("/public/api/[controller]/{itemId}/unavailable")]
    public IActionResult MarkItemAsUnavailable(Guid itemId)
    {
      _itemCatalogRepository.MarkItemAsUnavailable(itemId);
      return Ok();
    }
  }
}
