using System;
using System.Collections.Generic;

namespace TheWardrobe.API.ItemCatalog.Models
{
  public class QueryFilters
  {
    public Guid SellerIdInclude { get; set; }
    public Guid SellerIdExclude { get; set; }
    public float MinPrice { get; set; }
    public float MaxPrice { get; set; }
  }
}