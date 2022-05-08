using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using TheWardrobe.Helpers;

namespace TheWardrobe.API.Interfaces
{
  public class AccountDetailsInterface
  {
    private readonly Uri _serviceUri;

    public AccountDetailsInterface(IConfiguration configuration)
    {
      _serviceUri = configuration.GetServiceUri(ServiceNames.AccountService);
    }

    public async Task<string> GetAccountName(Guid accountId)
    {
      var url = _serviceUri.AppendPathSegments(new string[] {
                      "api",
                      accountId.ToString(),
                      "accountDetails",
                      "accountName" });

      return await url.GetStringAsync();
    }
  }
}