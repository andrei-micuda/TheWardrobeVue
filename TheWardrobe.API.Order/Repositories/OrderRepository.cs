using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Text;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using TheWardrobe.Helpers;
using TheWardrobe.CrossCutting.Helpers;
using TheWardrobe.API.Entities;

namespace TheWardrobe.API.Repositories
{
  public interface IOrderRepository
  {
    IEnumerable<Order> GetOrders(Guid accountId);
  }

  public class OrderRepository : IOrderRepository
  {
    private readonly IMapper _mapper;
    private readonly IDapperContext _dapperContext;

    public OrderRepository(IMapper mapper, IDapperContext dapperContext)
    {
      _mapper = mapper;
      _dapperContext = dapperContext;
    }

    public IEnumerable<Order> GetOrders(Guid accountId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.GetList<Order>(new { BuyerId = accountId });
    }
  }
}