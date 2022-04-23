using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Models;
using TheWardrobe.CrossCutting.Helpers;
using TheWardrobe.Helpers;

namespace TheWardrobe.API.Repositories
{
  public interface IAccountDetailsRepository
  {
    AccountDetails GetAccountDetails(Guid accountId);
    AccountDetails UpdateAccountDetails(AccountDetails model);
  }

  public class AccountDetailsRepository : IAccountDetailsRepository
  {
    private readonly IMapper _mapper;
    private readonly IDapperContext _dapperContext;

    public AccountDetailsRepository(IMapper mapper, IDapperContext dapperContext)
    {
      _mapper = mapper;
      _dapperContext = dapperContext;
    }

    public AccountDetails GetAccountDetails(Guid accountId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.Get<AccountDetails>(accountId);
    }

    public AccountDetails UpdateAccountDetails(AccountDetails model)
    {
      using var connection = _dapperContext.GetConnection();

      var rows = connection.Update(model);
      if (rows != 1)
      {
        throw new AppException("Account details update failed.");
      }
      return model;
    }
  }
}