using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWardrobe.API.Entities
{
  public enum OrderStatus
  {
    Pending,
    InProgress,
    Completed,
    Cancelled,
    Declined
  }

  [Table("order")]
  public class Order
  {
    [Column("id")]
    public Guid Id { get; set; }
    [Column("seller_id")]
    public Guid SellerId { get; set; }
    [Column("buyer_id")]
    public Guid BuyerId { get; set; }
    [Column("delivery_address_id")]
    public Guid DeliveryAddressId { get; set; }
    [Column("when_placed")]
    public DateTime WhenPlaced { get; set; }
    [Column("status")]
    public OrderStatus Status { get; set; }
  }
}