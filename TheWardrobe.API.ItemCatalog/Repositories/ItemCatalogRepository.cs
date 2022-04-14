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
using TheWardrobe.API.ItemCatalog.Entities;
using TheWardrobe.API.ItemCatalog.Models;
using TheWardrobe.API.ItemCatalog.Helpers;
using TheWardrobe.CrossCutting.Helpers;

namespace TheWardrobe.API.Repositories
{
  public interface IItemCatalogRepository
  {
    Item InsertItem(ItemRequestResponse model);
    IEnumerable<ItemRequestResponse> GetItems(Guid sellerId);
    ItemRequestResponse GetItem(Guid itemId);
    void UpdateItem(ItemRequestResponse model);
  }

  public class ItemCatalogRepository : IItemCatalogRepository
  {
    private readonly IMapper _mapper;
    private readonly IDapperContext _dapperContext;

    public ItemCatalogRepository(IMapper mapper, IDapperContext dapperContext)
    {
      _mapper = mapper;
      _dapperContext = dapperContext;
    }

    public Item InsertItem(ItemRequestResponse model)
    {
      var item = _mapper.Map<Item>(model);

      item.CategoryId = GetCategoryId(model.Category) ?? throw new AppException("Unknown category value");
      item.SizeId = GetSizeId(model.Size) ?? throw new AppException("Unknown size value");
      item.GenderId = GetGenderId(model.Gender) ?? throw new AppException("Unknown gender value");
      item.BrandId = GetBrandId(model.Brand) ?? throw new AppException("Unknown brand value");

      using var connection = _dapperContext.GetConnection();
      item.Id = connection.Insert<Guid, Item>(item);

      InsertItemImages(item.Id, model.Images);
      return item;
    }

    private void InsertItemImages(Guid itemId, IEnumerable<string> images)
    {
      using var connection = _dapperContext.GetConnection();

      foreach (var url in images)
      {
        connection.Execute(@"
          INSERT INTO item_image (item_id, url)
          VALUES (@itemId, @url)", new { itemId, url });
      }
    }

    private void ReplaceItemImages(Guid itemId, IEnumerable<string> images)
    {
      using var connection = _dapperContext.GetConnection();

      connection.Execute(@"
        DELETE FROM item_image
        WHERE item_id = @itemId", new { itemId });

      foreach (var url in images)
      {
        connection.Execute(@"
          INSERT INTO item_image (item_id, url)
          VALUES (@itemId, @url)", new { itemId, url });
      }
    }

    private IEnumerable<string> GetItemImages(Guid itemId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.Query<string>("SELECT url FROM item_image WHERE item_id = @itemId", new { itemId });
    }

    public IEnumerable<ItemRequestResponse> GetItems(Guid sellerId)
    {
      using var connection = _dapperContext.GetConnection();

      var items = connection.Query<ItemRequestResponse>(@"
        SELECT i.*, b.name AS brand, c.name AS category, g.name AS gender, s.name AS size
        FROM item i, brand b, category c, gender g, size s
        WHERE
          i.seller_id = @sellerId AND
          i.brand_id = b.id AND
          i.category_id = c.id AND
          i.gender_id = g.id AND
          i.size_id = s.id;
      ", new { sellerId });

      foreach (var item in items)
      {
        item.Images = GetItemImages(item.Id);
      }

      return items;
    }

    public IEnumerable<(int id, string name)> GetBrands()
    {
      using var connection = _dapperContext.GetConnection();

      var res = connection.Query<(int, string)>(@"
        SELECT id, name FROM brand");

      return res;
    }

    public int? GetCategoryId(string categoryName)
    {
      using var connection = _dapperContext.GetConnection();

      var categoryId = connection.ExecuteScalar<int?>(
        @"SELECT Id FROM category where name = @categoryName",
        new { categoryName });

      return categoryId;
    }

    public int? GetBrandId(string brandName)
    {
      using var connection = _dapperContext.GetConnection();

      var brandId = connection.ExecuteScalar<int?>(
        @"SELECT Id FROM brand where name = @brandName",
        new { brandName });

      return brandId;
    }

    public int? GetGenderId(string genderName)
    {
      using var connection = _dapperContext.GetConnection();

      var genderId = connection.ExecuteScalar<int?>(
        @"SELECT Id FROM gender where name = @genderName",
        new { genderName });

      return genderId;
    }

    public int? GetSizeId(string sizeName)
    {
      using var connection = _dapperContext.GetConnection();

      var sizeId = connection.ExecuteScalar<int?>(
        @"SELECT Id FROM size where name = @sizeName",
        new { sizeName });

      return sizeId;
    }

    public ItemRequestResponse GetItem(Guid itemId)
    {
      using var connection = _dapperContext.GetConnection();

      var item = connection.QueryFirstOrDefault<ItemRequestResponse>(@"
        SELECT i.*, b.name AS brand, c.name AS category, g.name AS gender, s.name AS size
        FROM item i, brand b, category c, gender g, size s
        WHERE
          i.id = @itemId AND
          i.brand_id = b.id AND
          i.category_id = c.id AND
          i.gender_id = g.id AND
          i.size_id = s.id;
      ", new { itemId });

      item.Images = GetItemImages(item.Id);

      return item;
    }

    public void UpdateItem(ItemRequestResponse model)
    {
      var item = _mapper.Map<Item>(model);

      item.CategoryId = GetCategoryId(model.Category) ?? throw new AppException("Unknown category value");
      item.SizeId = GetSizeId(model.Size) ?? throw new AppException("Unknown size value");
      item.GenderId = GetGenderId(model.Gender) ?? throw new AppException("Unknown gender value");
      item.BrandId = GetBrandId(model.Brand) ?? throw new AppException("Unknown brand value");

      using var connection = _dapperContext.GetConnection();
      connection.Execute(@"
        UPDATE item SET
          product_name = @ProductName,
          price = @Price,
          brand_id = @BrandId,
          gender_id = @GenderId,
          category_id = @CategoryId,
          size_id = @SizeId
        WHERE id = @Id", item);

      ReplaceItemImages(item.Id, model.Images);
      return;
    }
  }
}