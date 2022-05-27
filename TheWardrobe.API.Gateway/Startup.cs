using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using TheWardrobe.API.Gateway.Policies;
using TheWardrobe.Helpers;

namespace TheWardrobe.API.Gateway
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      var tokenKey = Configuration.GetSection("AppSettings").GetValue<string>("Secret");
      var key = Encoding.ASCII.GetBytes(tokenKey);
      services.AddAuthentication(options =>
        {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
          x.RequireHttpsMetadata = false;
          x.SaveToken = true;
          x.Events = new JwtBearerEvents()
          {
            OnTokenValidated = ctx =>
              {
                return Task.CompletedTask;
              }
          };
          x.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            ClockSkew = TimeSpan.Zero
          };
        });

      // Add the reverse proxy to capability to the server
      var proxyBuilder = services.AddReverseProxy()
        .LoadFromConfig(Configuration.GetSection("ReverseProxy"))
        .AddConfigFilter<MapServices>();

      services.AddAuthorization(options =>
      {
        options.AddPolicy("AllowEdit", policy =>
        {
          policy.RequireAuthenticatedUser();
          policy.AddRequirements(new AllowEditRequirement());
        });

        options.AddPolicy("OwnsItem", policy =>
        {
          policy.RequireAuthenticatedUser();
          policy.AddRequirements(new OwnsItemRequirement());
        });
      });

      services.AddScoped<IDapperContext, DapperContext>();
      services.AddScoped<IAuthorizationHandler, AllowEditHandler>();
      services.AddScoped<IAuthorizationHandler, OwnsItemHandler>();

      services.AddHttpContextAccessor();

      services.AddCors(options =>
      {
        options.AddPolicy(name: "allow-localhost-origins",
                              builder =>
                              {
                                builder.WithOrigins("https://localhost:8080");
                                // builder.WithOrigins("http://localhost:3000");
                              });
      });
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

        var serviceNames = new string[] { "Account", "ItemCatalog", "Order" };

        app.UseSwaggerUI(c =>
        {
          foreach (var serviceName in serviceNames)
          {
            c.SwaggerEndpoint($"/public/api/{serviceName}/swagger/v1/swagger.json", $"TheWardrobe.API.{serviceName} v1");
          }
          c.RoutePrefix = "swagger";
        });
      }

      app.UseHttpsRedirection();

      app.UseAuthentication();
      app.UseRouting();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapReverseProxy();
        endpoints.MapGet("/test-tye", async context =>
        {
          var url = Configuration.GetServiceUri("thewardrobe-account");
          await context.Response.WriteAsync(url.ToString());
        });
      });
    }
  }
}
