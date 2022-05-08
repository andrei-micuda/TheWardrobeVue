using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using TheWardrobe.API.Entities;
using TheWardrobe.Helpers;

namespace TheWardrobe.API.Interfaces
{
  public class DeliveryAddressInterface
  {
    private readonly Uri _serviceUri;

    public DeliveryAddressInterface(IConfiguration configuration)
    {
      _serviceUri = configuration.GetServiceUri(ServiceNames.AccountService);
    }

    public async Task<DeliveryAddress> GetDeliveryAddress(Guid deliveryAddressId)
    {
      var url = _serviceUri.AppendPathSegments(new string[] {
                      "api",
                      "deliveryAddress",
                      deliveryAddressId.ToString() });

      return await url.GetJsonAsync<DeliveryAddress>();
    }
  }
}