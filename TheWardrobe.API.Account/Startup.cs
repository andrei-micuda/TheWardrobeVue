using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TheWardrobe.API.Helpers;
using TheWardrobe.API.Middleware;
using TheWardrobe.API.Publishers;
using TheWardrobe.API.Repositories;
using TheWardrobe.Helpers;

namespace TheWardrobe.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "TheWardrobe.API", Version = "v1" });
      });

      // configure strongly typed settings objects
      services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

      services.AddScoped<IDapperContext, DapperContext>();

      // configure DI for application services
      services.AddScoped<IAccountRepository, AccountRepository>();
      services.AddScoped<IAccountDetailsRepository, AccountDetailsRepository>();
      services.AddScoped<IDeliveryAddressRepository, DeliveryAddressRepository>();
      services.AddScoped<ISendEmailPublisher, SendEmailPublisher>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger(c =>
        {
          c.RouteTemplate = "public/api/Account/swagger/{documentName}/swagger.json";
        });
        // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TheWardrobe.API v1"));
      }

      app.UseHttpsRedirection();


      app.UseCors("allow-localhost-origins");

      // global error handler
      app.UseMiddleware<ErrorHandlerMiddleware>();
      // custom jwt auth middleware
      // app.UseMiddleware<JwtMiddleware>();

      app.UseRouting();
      // app.UseAuthentication();
      // app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
