using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Models;
using TheWardrobe.Helpers;

namespace TheWardrobe.API.Interfaces
{
  public class ItemCatalogInterface
  {
    private readonly Uri _serviceUri;

    public ItemCatalogInterface(IConfiguration configuration)
    {
      _serviceUri = configuration.GetServiceUri(ServiceNames.ItemCatalogService);
    }

    public async Task MarkItemAsUnavailable(Guid itemId)
    {
      var url = _serviceUri.AppendPathSegments(new string[] {
                      "public",
                      "api",
                      "itemCatalog",
                      itemId.ToString(),
                      "unavailable"  });

      await url.PatchAsync();
    }

    public async Task<ItemRequestResponse> GetItem(Guid itemId)
    {
      var url = _serviceUri.AppendPathSegments(new string[] {
                      "public",
                      "api",
                      "itemCatalog",
                      itemId.ToString() });

      return await url.GetJsonAsync<ItemRequestResponse>();
    }
  }
}