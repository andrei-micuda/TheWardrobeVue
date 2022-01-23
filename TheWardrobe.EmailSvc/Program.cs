using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TheWardrobe.CrossCutting.Messages;
using TheWardrobe.EmailSvc.Consumers;

namespace TheWardrobe.EmailSvc
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
                {
                  EndpointConvention.Map<SendEmail>(new Uri("queue:send-email-queue"));
                  services.AddMassTransit(x =>
                      {
                        x.AddConsumer<SendEmailConsumer>()
                        .Endpoint(e =>
                        {
                          e.Name = "send-email";
                        });

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
