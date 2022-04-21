using System;
using System.Collections.Generic;

namespace TheWardrobe.API.ItemCatalog.Models
{
  public class QueryFilters
  {
    public Guid? RequesterId { get; set; }
    public Guid? SellerIdInclude { get; set; }
    public Guid? SellerIdExclude { get; set; }
    public IEnumerable<string> Brands { get; set; }
    public IEnumerable<string> Categories { get; set; }
    public IEnumerable<string> Genders { get; set; }
    public IEnumerable<string> Sizes { get; set; }
    public float? MinPrice { get; set; }
    public float? MaxPrice { get; set; }
    public bool OnlyFavorites { get; set; }
    public string OrderBy { get; set; } = "whenAdded";
    public string Order { get; set; } = "desc"; // asc or desc
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
  }
}