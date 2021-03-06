using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TheWardrobe.API.Migrations;

namespace TheWardrobe.API.Gateway
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var serviceProvider = CreateServices();

      // Put the database update into a scope to ensure
      // that all resources will be disposed.
      using (var scope = serviceProvider.CreateScope())
      {
        UpdateDatabase(scope.ServiceProvider);
      }

      Log.Logger = new LoggerConfiguration()
                  .MinimumLevel.Verbose()
                  .Enrich.FromLogContext()
                  .WriteTo.Console()
                 .CreateLogger();

      CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Configure the dependency injection services
    /// </summary>
    private static IServiceProvider CreateServices()
    {
      return new ServiceCollection()
          // Add common FluentMigrator services
          .AddFluentMigratorCore()
          .ConfigureRunner(rb => rb
              // Add Postgres support to FluentMigrator
              .AddPostgres()
              // Set the connection string
              .WithGlobalConnectionString("User ID=postgres;Password=p@ssw0rd;Host=localhost;Port=5432;Database=the_wardrobe;")
              // Define the assembly containing the migrations
              .ScanIn(typeof(Initial).Assembly).For.Migrations())
          // Enable logging to console in the FluentMigrator way
          .AddLogging(lb => lb.AddFluentMigratorConsole())
          // Build the service provider
          .BuildServiceProvider(false);
    }

    /// <summary>
    /// Update the database
    /// </summary>
    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
      // Instantiate the runner
      var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

      // runner.MigrateDown(0);

      // revert before Order migration
      // runner.MigrateDown(202204200000);

      // Execute the migrations
      runner.MigrateUp();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
