using System;
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

    [HttpPost]
    [Route("/api/[controller]")]
    public IActionResult Post(ItemRequestResponse model)
    {
      var item = _itemCatalogRepository.InsertItem(model);
      return Ok(item);
    }

    [HttpGet]
    [Route("/api/[controller]/{sellerId}")]
    public IActionResult Get(Guid sellerId)
    {
      return Ok(_itemCatalogRepository.GetItems(sellerId));
    }
  }
}
