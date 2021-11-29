using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
using TheWardrobe.API.Repositories;

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

      services.AddCors(options =>
      {
        options.AddPolicy(name: "allow-localhost-origins",
                              builder =>
                              {
                                builder.WithOrigins("https://localhost:8080");
                              });
      });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "TheWardrobe.API", Version = "v1" });
      });

      // configure strongly typed settings objects
      services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

      services.AddScoped<IDapperContext, DapperContext>();

      // configure DI for application services
      services.AddScoped<IAccountRepository, AccountRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TheWardrobe.API v1"));
      }

      app.UseHttpsRedirection();


      app.UseCors("allow-localhost-origins");

      // global error handler
      app.UseMiddleware<ErrorHandlerMiddleware>();
      // custom jwt auth middleware
      // app.UseMiddleware<JwtMiddleware>();

      app.UseAuthentication();
      app.UseRouting();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
