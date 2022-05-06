using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Yarp.ReverseProxy.Configuration;

namespace TheWardrobe.Helpers
{
  public class MapServices : IProxyConfigFilter
  {
    private readonly IConfiguration Configuration;

    public MapServices(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    // This sample looks at the destination addresses and any of the form {{key}} will be modified, looking up the key
    // as an environment variable. This is useful when hosted in Azure etc, as it enables a simple way to replace
    // destination addresses via the management console
    public ValueTask<ClusterConfig> ConfigureClusterAsync(ClusterConfig origCluster, CancellationToken cancel)
    {
      // Each cluster has a dictionary of destinations, which is read-only, so we'll create a new one with our updates 
      var newDests = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase);

      foreach (var d in origCluster.Destinations)
      {
        // Get the current destination, which should be a service name
        var serviceName = d.Value.Address;

        // Get service Uri using Tye service discovery
        var newAddress = Configuration.GetServiceUri(serviceName).ToString();

        if (string.IsNullOrWhiteSpace(newAddress))
        {
          throw new ArgumentException($"Configuration Filter Error: Substitution for service name '{serviceName}' in cluster '{d.Key}' not found in tye service discovery.");
        }

        // using c# 9 "with" to clone and initialize a new record
        var modifiedDest = d.Value with { Address = newAddress };
        newDests.Add(d.Key, modifiedDest);
      }

      return new ValueTask<ClusterConfig>(origCluster with { Destinations = newDests });
    }

    public ValueTask<RouteConfig> ConfigureRouteAsync(RouteConfig route, ClusterConfig cluster, CancellationToken cancel)
    {
      return new ValueTask<RouteConfig>(route);
    }
  }
}