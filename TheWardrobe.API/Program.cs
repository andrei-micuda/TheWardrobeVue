using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TheWardrobe.API.Helpers;
using TheWardrobe.API.Migrations;

namespace TheWardrobe.API
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

      SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);

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
              .WithGlobalConnectionString("User ID=micu;Password=p@ssw0rd;Host=172.17.0.3;Port=5432;Database=the_wardrobe;")
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

      // Execute the migrations
      runner.MigrateUp();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
