using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheWardrobe.API.Entities
{

  [Table("favorite")]
  public class Favorite
  {
    [Column("account_id")]
    public Guid AccountId { get; set; }
    [Column("item_id")]
    public Guid ItemId { get; set; }
  }
}