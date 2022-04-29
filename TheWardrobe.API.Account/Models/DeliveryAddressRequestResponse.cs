using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWardrobe.API.Models
{
  public class DeliveryAddressRequestResponse
  {
    public Guid Id { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
  }
}