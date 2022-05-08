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
  public interface IFavoritesRepository
  {
    void Add(Guid accountId, FavoritesRequest model);
    void Remove(Guid accountId, FavoritesRequest model);
    bool CheckIsFavorite(Guid accountId, Guid itemId);
  }

  public class FavoritesRepository : IFavoritesRepository
  {
    private readonly IMapper _mapper;
    private readonly IDapperContext _dapperContext;

    public FavoritesRepository(IMapper mapper, IDapperContext dapperContext)
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

    public bool CheckIsFavorite(Guid accountId, Guid itemId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.ExecuteScalar<bool>(@"
        SELECT COUNT(*) > 0
        FROM favorite f
        WHERE f.item_id = @itemId
        AND f.account_id = @accountId;", new { itemId, accountId });
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