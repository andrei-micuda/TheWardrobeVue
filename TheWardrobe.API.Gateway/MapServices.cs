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