using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWardrobe.API.Cart.Models;
using TheWardrobe.API.Cart.Repositories;
using TheWardrobe.CrossCutting.Helpers;

namespace TheWardrobe.API.Cart.Controllers
{
  [ApiController]
  // [Route("/api/[controller]/[action]")]
  public class FavoritesController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<FavoritesController>();
    private readonly ICartRepository _cartRepository;

    public FavoritesController(ICartRepository cartRepository)
    {
      _cartRepository = cartRepository;
    }

    [HttpGet]
    [Route("/api/{accountId}/favorites/{itemId}")]
    public IActionResult GetFavoriteStatus(Guid accountId, Guid itemId)
    {
      var isFavorite = _cartRepository.CheckIsFavorite(accountId, itemId);
      return Ok(new { isFavorite });
    }
    [HttpPost]
    [Route("/api/{accountId}/favorites")]
    public IActionResult Post(Guid accountId, FavoritesRequest model)
    {
      _cartRepository.Add(accountId, model);
      return Ok();
    }

    [HttpDelete]
    [Route("/api/{accountId}/favorites")]
    public IActionResult Delete(Guid accountId, FavoritesRequest model)
    {
      _cartRepository.Remove(accountId, model);
      return Ok();
    }
  }
}
