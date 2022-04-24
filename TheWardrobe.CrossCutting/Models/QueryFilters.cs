using System;
using System.Collections.Generic;

namespace TheWardrobe.API.Models
{
  public class QueryFilters
  {
    public string OrderBy { get; set; } = "whenAdded";
    public string Order { get; set; } = "desc"; // asc or desc
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
  }
}