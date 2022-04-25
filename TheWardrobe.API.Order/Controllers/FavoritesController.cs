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
  // [Route("/api/[controller]/[action]")]
  public class FavoritesController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<FavoritesController>();
    private readonly IFavoritesRepository _favoritesRepository;

    public FavoritesController(IFavoritesRepository favoritesRepository)
    {
      _favoritesRepository = favoritesRepository;
    }

    [HttpGet]
    [Route("/api/{accountId}/favorites/{itemId}")]
    public IActionResult GetFavoriteStatus(Guid accountId, Guid itemId)
    {
      var isFavorite = _favoritesRepository.CheckIsFavorite(accountId, itemId);
      return Ok(new { isFavorite });
    }
    [HttpPost]
    [Route("/api/{accountId}/favorites")]
    public IActionResult Post(Guid accountId, FavoritesRequest model)
    {
      _favoritesRepository.Add(accountId, model);
      return Ok();
    }

    [HttpDelete]
    [Route("/api/{accountId}/favorites")]
    public IActionResult Delete(Guid accountId, FavoritesRequest model)
    {
      _favoritesRepository.Remove(accountId, model);
      return Ok();
    }
  }
}
