using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheWardrobe.API.Entities
{
  [Table("account")]
  public class AccountDetails
  {
    [Column("id")]
    public Guid Id { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("phone_number")]
    public string PhoneNumber { get; set; }
    [Column("first_name")]
    public string FirstName { get; set; }
    [Column("last_name")]
    public string LastName { get; set; }
  }
}