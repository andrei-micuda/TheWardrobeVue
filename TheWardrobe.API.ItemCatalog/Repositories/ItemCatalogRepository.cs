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
    ItemListResponse GetItems(QueryFilters filters);
    ItemRequestResponse GetItem(Guid itemId);
    void UpdateItem(ItemRequestResponse model);
    void DeleteItem(Guid itemId);
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

      item.CategoryId = GetCategoryId(model.Category);
      item.SizeId = GetSizeId(model.Size);
      item.GenderId = GetGenderId(model.Gender);
      item.BrandId = GetBrandId(model.Brand);

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

    public ItemListResponse GetItems(QueryFilters filters)
    {
      var res = new ItemListResponse();
      string sellerIdOperator = "";
      Guid sellerId = new();
      if (filters.SellerIdInclude != null)
      {
        sellerIdOperator = "=";
        sellerId = filters.SellerIdInclude.Value;
      }
      if (filters.SellerIdExclude != null)
      {
        sellerIdOperator = "<>";
        sellerId = filters.SellerIdExclude.Value;
      }

      var brandFilteringSql = "";
      IEnumerable<int> includedBrandIds = null;
      if (filters.Brands != null)
      {
        includedBrandIds = filters.Brands.Select(b => GetBrandId(b)).ToList();
        brandFilteringSql = $"i.brand_id = ANY(@includedBrandIds) AND";
      }

      var categoryFilteringSql = "";
      IEnumerable<int> includedCategoryIds = null;
      if (filters.Categories != null)
      {
        includedCategoryIds = filters.Categories.Select(c => GetCategoryId(c)).ToList();
        categoryFilteringSql = $"i.category_id = ANY(@includedCategoryIds) AND";
      }

      var genderFilteringSql = "";
      IEnumerable<int> includedGenderIds = null;
      if (filters.Genders != null)
      {
        includedGenderIds = filters.Genders.Select(g => GetGenderId(g)).ToList();
        genderFilteringSql = $"i.gender_id = ANY(@includedGenderIds) AND";
      }

      var sizeFilteringSql = "";
      IEnumerable<int> includedSizeIds = null;
      if (filters.Sizes != null)
      {
        includedSizeIds = filters.Sizes.Select(s => GetSizeId(s)).ToList();
        sizeFilteringSql = $"i.size_id = ANY(@includedSizeIds) AND";
      }

      var minPriceFilteringSql = "";
      if (filters.MinPrice.HasValue)
      {
        minPriceFilteringSql = $"i.price >= @MinPrice AND";
      }

      var maxPriceFilteringSql = "";
      if (filters.MaxPrice.HasValue)
      {
        maxPriceFilteringSql = $"i.price <= @MaxPrice AND";
      }

      using var connection = _dapperContext.GetConnection();

      res.Items = connection.Query<ItemRequestResponse>(@$"
        SELECT i.*, b.name AS brand, c.name AS category, g.name AS gender, s.name AS size
        FROM item i, brand b, category c, gender g, size s
        WHERE
          {(sellerIdOperator != "" ? $"i.seller_id {sellerIdOperator} @sellerId AND" : "")}
          {minPriceFilteringSql}
          {maxPriceFilteringSql}
          {brandFilteringSql}
          {categoryFilteringSql}
          {genderFilteringSql}
          {sizeFilteringSql}
          i.brand_id = b.id AND
          i.category_id = c.id AND
          i.gender_id = g.id AND
          i.size_id = s.id;
      ", new
      {
        sellerId,
        filters.MinPrice,
        filters.MaxPrice,
        includedBrandIds,
        includedCategoryIds,
        includedGenderIds,
        includedSizeIds
      });

      foreach (var item in res.Items)
      {
        item.Images = GetItemImages(item.Id);
      }

      return res;
    }

    public IEnumerable<(int id, string name)> GetBrands()
    {
      using var connection = _dapperContext.GetConnection();

      var res = connection.Query<(int, string)>(@"
        SELECT id, name FROM brand");

      return res;
    }

    public int GetCategoryId(string categoryName)
    {
      using var connection = _dapperContext.GetConnection();

      var categoryId = connection.ExecuteScalar<int?>(
        @"SELECT Id FROM category where name = @categoryName",
        new { categoryName });

      if (categoryId.HasValue)
        return categoryId.Value;

      throw new AppException("Unknown category value.");
    }

    public int GetBrandId(string brandName)
    {
      using var connection = _dapperContext.GetConnection();

      var brandId = connection.ExecuteScalar<int?>(
        @"SELECT Id FROM brand where name = @brandName",
        new { brandName });

      if (brandId.HasValue)
        return brandId.Value;

      throw new AppException("Unknown brand value.");
    }

    public int GetGenderId(string genderName)
    {
      using var connection = _dapperContext.GetConnection();

      var genderId = connection.ExecuteScalar<int?>(
        @"SELECT Id FROM gender where name = @genderName",
        new { genderName });

      if (genderId.HasValue)
        return genderId.Value;

      throw new AppException("Unknown gender value.");
    }

    public int GetSizeId(string sizeName)
    {
      using var connection = _dapperContext.GetConnection();

      var sizeId = connection.ExecuteScalar<int?>(
        @"SELECT Id FROM size where name = @sizeName",
        new { sizeName });

      if (sizeId.HasValue)
        return sizeId.Value;

      throw new AppException("Unknown size value.");
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

      item.CategoryId = GetCategoryId(model.Category);
      item.SizeId = GetSizeId(model.Size);
      item.GenderId = GetGenderId(model.Gender);
      item.BrandId = GetBrandId(model.Brand);

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

    public void DeleteItem(Guid itemId)
    {
      using var connection = _dapperContext.GetConnection();

      connection.Delete<Item>(itemId);
    }
  }
}