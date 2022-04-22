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
      // Add the reverse proxy to capability to the server
      var proxyBuilder = services.AddReverseProxy();
      // Initialize the reverse proxy from the "ReverseProxy" section of configuration
      proxyBuilder.LoadFromConfig(Configuration.GetSection("ReverseProxy"));

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
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapReverseProxy();
      });
    }
  }
}
