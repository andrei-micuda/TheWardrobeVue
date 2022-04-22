using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWardrobe.API.Entities
{
  [Table("delivery_address")]
  public class DeliveryAddress
  {
    [Column("id")]
    public Guid Id { get; set; }
    [Column("account_id")]
    public Guid AccountId { get; set; }
    [Column("address")]
    public string Address { get; set; }
    [Column("city")]
    public string City { get; set; }
    [Column("country")]
    public string Country { get; set; }
    [Column("postal_code")]
    public string PostalCode { get; set; }
  }
}