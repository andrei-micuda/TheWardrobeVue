using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Npgsql;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Helpers;
using TheWardrobe.API.Models.Accounts;
using Xunit;

namespace TheWardrobe.API.Tests
{
  public class AccountControllerTests : IClassFixture<ApiWebApplicationFactory>, IDisposable
  {
    readonly HttpClient _client;
    private readonly string _connString;

    public AccountControllerTests(ApiWebApplicationFactory fixture)
    {
      _client = fixture.CreateClient();
      _connString = fixture.Configuration["ConnectionString"];
    }

    public void Dispose()
    {
      using var conn = new NpgsqlConnection(_connString);
      conn.Execute("TRUNCATE account, refresh_token, role");
    }

    [Fact]
    public async Task GET_hello()
    {
      var response = await _client.GetAsync("/Accounts/hello");
      response.StatusCode.Should().Be(HttpStatusCode.OK);
      // var forecast = JsonConvert.DeserializeObject<WeatherForecast[]>(await response.Content.ReadAsStringAsync());
      // forecast.Should().HaveCount(5);
    }

    [Fact]
    public async Task POST_register()
    {
      SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
      var reqBody = new RegisterRequest { Email = "email@example.com", Password = "password", ConfirmPassword = "password" };
      var stringContent = new StringContent(JsonConvert.SerializeObject(reqBody), Encoding.UTF8, "application/json");
      System.Console.WriteLine(await stringContent.ReadAsStringAsync());
      var response = await _client.PostAsync("/Accounts/register", stringContent);
      System.Console.WriteLine(await response.Content.ReadAsStringAsync());
      response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
  }
}