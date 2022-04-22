using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TheWardrobe.API.Helpers;
using TheWardrobe.CrossCutting.Messages;

namespace TheWardrobe.API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);

      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureServices((hostContext, services) =>
              {
                EndpointConvention.Map<SendEmail>(new Uri("queue:send-email"));
                services.AddMassTransit(x =>
                {
                  // x.AddConsumer<MessageConsumer>();

                  x.UsingRabbitMq((context, cfg) =>
                  {
                    cfg.ConfigureEndpoints(context);
                  });
                });

                services.AddMassTransitHostedService();
              })
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
