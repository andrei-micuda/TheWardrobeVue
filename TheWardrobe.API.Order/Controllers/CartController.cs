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

    public CartController(ICartRepository cartRepository)
    {
      _cartRepository = cartRepository;
    }

    [HttpGet("{itemId}")]
    public IActionResult GetFavoriteStatus(Guid accountId, Guid itemId)
    {
      var isInCart = _cartRepository.CheckIsInCart(accountId, itemId);
      return Ok(new { isInCart });
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
