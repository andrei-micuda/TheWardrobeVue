using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWardrobe.API.Models;
using TheWardrobe.API.Repositories;

namespace TheWardrobe.API.Controllers
{
  [Route("/api/{accountId}/[controller]")]
  public class DeliveryAddressController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<DeliveryAddressController>();
    private readonly IDeliveryAddressRepository _repository;

    public DeliveryAddressController(IDeliveryAddressRepository repository)
    {
      _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll(Guid accountId)
    {
      var res = _repository.GetAll(accountId);
      return Ok(res);
    }

    [HttpPost]
    public IActionResult Post(Guid accountId, [FromBody] DeliveryAddressRequestResponse model)
    {
      var res = _repository.AddDeliveryAddress(accountId, model);
      return Ok(res);
    }

    [HttpPut("{deliveryAddressId}")]
    public IActionResult Put(Guid accountId, Guid deliveryAddressId, [FromBody] DeliveryAddressRequestResponse model)
    {
      var res = _repository.UpdateDeliveryAddress(accountId, deliveryAddressId, model);
      return Ok(res);
    }

    [HttpDelete("{deliveryAddressId}")]
    public IActionResult Delete(Guid accountId, Guid deliveryAddressId)
    {
      _repository.DeleteDeliveryAddress(accountId, deliveryAddressId);
      return Ok();
    }
  }
}