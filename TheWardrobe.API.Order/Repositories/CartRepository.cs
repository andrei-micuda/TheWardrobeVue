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
using TheWardrobe.API.Models;
using TheWardrobe.API.Entities;

namespace TheWardrobe.API.Repositories
{
  public interface ICartRepository
  {
    void Add(Guid accountId, CartRequest model);
    void Remove(Guid accountId, CartRequest model);
    bool CheckIsInCart(Guid accountId, Guid itemId);
    IEnumerable<Guid> GetCart(Guid accountId);
  }

  public class CartRepository : ICartRepository
  {
    private readonly IMapper _mapper;
    private readonly IDapperContext _dapperContext;

    public CartRepository(IMapper mapper, IDapperContext dapperContext)
    {
      _mapper = mapper;
      _dapperContext = dapperContext;
    }

    public void Add(Guid accountId, CartRequest model)
    {
      var cart = _mapper.Map<Cart>(model);
      cart.AccountId = accountId;
      using var connection = _dapperContext.GetConnection();

      connection.Execute("INSERT INTO cart VALUES (@accountId, @itemId);", cart);
    }

    public bool CheckIsInCart(Guid accountId, Guid itemId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.ExecuteScalar<bool>(@"
        SELECT COUNT(*) > 0
        FROM cart c
        WHERE c.item_id = @itemId
        AND c.account_id = @accountId;", new { itemId, accountId });
    }

    public IEnumerable<Guid> GetCart(Guid accountId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.Query<Guid>("SELECT item_id from cart WHERE account_id = @accountId", new { accountId });
    }

    public void Remove(Guid accountId, CartRequest model)
    {
      var cart = _mapper.Map<Cart>(model);
      cart.AccountId = accountId;
      using var connection = _dapperContext.GetConnection();

      connection.Execute(@"
        DELETE FROM cart
        WHERE account_id = @accountId
        AND item_id = @itemId;", cart);
    }
  }
}