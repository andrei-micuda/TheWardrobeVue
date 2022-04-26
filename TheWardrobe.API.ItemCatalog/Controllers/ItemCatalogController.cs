using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWardrobe.API.Models;
using TheWardrobe.API.Repositories;
using TheWardrobe.CrossCutting.Helpers;

namespace TheWardrobe.API.ItemCatalog.Controllers
{
  [ApiController]
  // [Route("/api/[controller]/[action]")]
  public class ItemCatalogController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<ItemCatalogController>();
    private readonly IItemCatalogRepository _itemCatalogRepository;
    private readonly IAccountDetailsRepository _accountDetailsRepository;

    public ItemCatalogController(IItemCatalogRepository itemCatalogRepository, IAccountDetailsRepository accountDetailsRepository)
    {
      _itemCatalogRepository = itemCatalogRepository;
      _accountDetailsRepository = accountDetailsRepository;
    }

    [HttpGet]
    [Route("/api/[controller]")]
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
    [Route("/api/[controller]")]
    public IActionResult Post(ItemRequestResponse model)
    {
      var item = _itemCatalogRepository.InsertItem(model);
      return Ok(item);
    }

    [HttpGet]
    [Route("/api/[controller]/{itemId}")]
    public IActionResult GetItemById(Guid itemId)
    {
      var item = _itemCatalogRepository.GetItem(itemId);
      item.Seller = _accountDetailsRepository.GetAccountName(item.SellerId);
      return Ok();
    }


    [HttpPut]
    [Route("/api/[controller]/{itemId}")]
    public IActionResult UpdateItemById(Guid itemId, ItemRequestResponse model)
    {
      model.Id = itemId;
      _itemCatalogRepository.UpdateItem(model);
      return Ok();
    }

    [HttpDelete]
    [Route("/api/[controller]/{itemId}")]
    public IActionResult DeleteItemById(Guid itemId)
    {
      _itemCatalogRepository.DeleteItem(itemId);
      return Ok();
    }
  }
}
