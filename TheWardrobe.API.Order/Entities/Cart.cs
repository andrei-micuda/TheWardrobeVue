using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheWardrobe.API.Entities
{

  [Table("cart")]
  public class Cart
  {
    [Column("account_id")]
    public Guid AccountId { get; set; }
    [Column("item_id")]
    public Guid ItemId { get; set; }
  }
}