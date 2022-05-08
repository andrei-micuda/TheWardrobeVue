using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Models;
using TheWardrobe.API.Repositories;

namespace TheWardrobe.API.Controllers
{
  [Route("/public/api/{accountId}/[controller]")]
  public class AccountDetailsController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<AccountDetailsController>();
    private readonly IAccountDetailsRepository _repository;

    public AccountDetailsController(IAccountDetailsRepository repository)
    {
      _repository = repository;
    }

    [HttpGet]
    public IActionResult Get(Guid accountId)
    {
      var res = _repository.GetAccountDetails(accountId);
      return Ok(res);
    }

    [HttpPut]
    public IActionResult Put(Guid accountId, [FromBody] AccountDetails model)
    {
      model.Id = accountId;
      var res = _repository.UpdateAccountDetails(model);
      return Ok(res);
    }

    // internal endpoints
    [HttpGet]
    [Route("/api/{accountId}/[controller]/accountName")]
    public IActionResult GetAccountName(Guid accountId)
    {
      var res = _repository.GetAccountName(accountId);
      return Ok(res);
    }
  }
}