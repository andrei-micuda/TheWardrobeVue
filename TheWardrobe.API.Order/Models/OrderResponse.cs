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
    public DeliveryAddress DeliveryAddress { get; set; }
    public float Total { get; set; }
    public DateTime WhenPlaced { get; set; }
    public OrderStatus Status { get; set; }
    public IEnumerable<ItemRequestResponse> OrderItems { get; set; }
    public float? BuyerRating { get; set; }
    public int OrderBuyerRating { get; set; }
    public float? SellerRating { get; set; }
    public int OrderSellerRating { get; set; }
  }
}