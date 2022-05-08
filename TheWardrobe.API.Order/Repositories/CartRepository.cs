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
using TheWardrobe.API.Models;
using TheWardrobe.API.Entities;

namespace TheWardrobe.API.Repositories
{
  public interface ICartRepository
  {
    void Add(Guid accountId, CartRequest model);
    void Remove(Guid accountId, CartRequest model);
    bool CheckIsInCart(Guid accountId, Guid itemId);
    IEnumerable<Guid> GetCart(Guid accountId, Guid? sellerId);
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

    public IEnumerable<Guid> GetCart(Guid accountId, Guid? sellerId)
    {
      using var connection = _dapperContext.GetConnection();

      string sql;

      if (sellerId == null)
      {
        sql = @"
        SELECT item_id
        FROM cart
        WHERE account_id = @accountId";
      }
      else
      {
        sql = @"
        SELECT c.item_id
        FROM cart c, item i
        WHERE c.item_id = i.id
        AND i.seller_id = @sellerId
        AND account_id = @accountId";
      }

      return connection.Query<Guid>(sql, new { accountId, sellerId });
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