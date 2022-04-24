using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWardrobe.API.Entities;

namespace TheWardrobe.API.Models
{
  public class OrderResponse
  {
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public string Seller { get; set; }
    public Guid BuyerId { get; set; }
    public string Buyer { get; set; }
    public DeliveryAddressRequestResponse DeliveryAddress { get; set; }
    public int Total { get; set; }
    public DateTime WhenPlaced { get; set; }
    public OrderStatus Status { get; set; }
    public IEnumerable<ItemRequestResponse> OrderItems { get; set; }
  }
}