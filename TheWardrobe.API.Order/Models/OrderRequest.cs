using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWardrobe.API.Models
{
  public class OrderRequest
  {
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public Guid DeliveryAddressId { get; set; }
    public IEnumerable<Guid> Items { get; set; }
  }
}