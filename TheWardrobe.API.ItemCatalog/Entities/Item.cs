using System;
using Dapper;

namespace TheWardrobe.API.ItemCatalog.Entities
{
  [Table("item")]
  public class Item
  {
    [Column("id")]
    public Guid Id { get; set; }
    [Column("seller_id")]
    public Guid SellerId { get; set; }
    [Column("product_name")]
    public string ProductName { get; set; }
    [Column("price")]
    public float Price { get; set; }
    [Column("brand_id")]
    public int BrandId { get; set; }
    [Column("gender_id")]
    public int GenderId { get; set; }
    [Column("category_id")]
    public int CategoryId { get; set; }
    [Column("size_id")]
    public int SizeId { get; set; }
  }
}