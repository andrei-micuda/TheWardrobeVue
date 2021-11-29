using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace TheWardrobe.API.Tests
{
  public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
  {
    public IConfiguration Configuration { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      builder.ConfigureAppConfiguration(config =>
      {
        Configuration = new ConfigurationBuilder()
          .AddJsonFile("settings.json")
          .Build();

        config.AddConfiguration(Configuration);
      });

      builder.ConfigureTestServices(services =>
      {
        // services.AddTransient<IWeatherForecastConfigService, WeatherForecastConfigStub>();
      });

    }
  }
}