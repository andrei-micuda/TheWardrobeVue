using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWardrobe.API.Entities;

namespace TheWardrobe.API.Models
{
  public class OrderQueryFilters : QueryFilters
  {
    public OrderType Type { get; set; }
  }

  public enum OrderType
  {
    Incoming,
    Outgoing
  }
}