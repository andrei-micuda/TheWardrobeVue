using System;

namespace TheWardrobe.API.Models
{
  public class OnPostItemCatalogRequest
  {
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public string ProductName { get; set; }
    public float Price { get; set; }
    public string Brand { get; set; }
    public string Gender { get; set; }
    public string Category { get; set; }
    public string Size { get; set; }
  }
}