﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWardrobe.API.ItemCatalog.Models;
using TheWardrobe.API.Repositories;

namespace TheWardrobe.API.ItemCatalog.Controllers
{
  [ApiController]
  // [Route("/api/[controller]/[action]")]
  public class ItemCatalogController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<ItemCatalogController>();
    private readonly IItemCatalogRepository _itemCatalogRepository;

    public ItemCatalogController(IItemCatalogRepository itemCatalogRepository)
    {
      _itemCatalogRepository = itemCatalogRepository;
    }

    [HttpGet]
    [Route("/api/[controller]")]
    public IActionResult GetItems([FromQuery] QueryFilters filters)
    {
      // sanity check query parameters
      if (filters.SellerIdExclude != Guid.Empty && filters.SellerIdInclude != Guid.Empty)
      {
        // if both are provided return BadRequest
        return BadRequest("Please only provide one of the following query parameters: sellerIdInclude, sellerIdExclude");
      }
      var items = _itemCatalogRepository.GetItems(filters);
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
      return Ok(_itemCatalogRepository.GetItem(itemId));
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
