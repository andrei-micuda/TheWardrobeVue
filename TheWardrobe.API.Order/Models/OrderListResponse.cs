using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWardrobe.API.Models
{
  public class OrderListResponse
  {
    public IEnumerable<OrderResponse> Orders { get; set; }
    public int NumOrders { get; set; }
  }
}