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
using TheWardrobe.API.Cart.Models;
using TheWardrobe.API.Cart.Entities;

namespace TheWardrobe.API.Cart.Repositories
{
  public interface ICartRepository
  {
    void Add(Guid accountId, FavoritesRequest model);
    void Remove(Guid accountId, FavoritesRequest model);
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

    public void Add(Guid accountId, FavoritesRequest model)
    {
      var fav = _mapper.Map<Favorite>(model);
      fav.AccountId = accountId;
      using var connection = _dapperContext.GetConnection();

      connection.Execute("INSERT INTO favorite VALUES (@accountId, @itemId);", fav);
    }

    public void Remove(Guid accountId, FavoritesRequest model)
    {
      var fav = _mapper.Map<Favorite>(model);
      fav.AccountId = accountId;
      using var connection = _dapperContext.GetConnection();

      connection.Execute(@"
        DELETE FROM favorite
        WHERE account_id = @accountId
        AND item_id = @itemId;", fav);
    }
  }
}