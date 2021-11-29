using Microsoft.Extensions.Configuration;
using Npgsql;

namespace TheWardrobe.API.Helpers
{
  public interface IDapperContext
  {
    public NpgsqlConnection GetConnection();
  }

  public class DapperContext : IDapperContext
  {
    private readonly IConfiguration _configuration;

    public DapperContext(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public NpgsqlConnection GetConnection()
    {
      return new NpgsqlConnection(_configuration["ConnectionString"]);
    }
  }
}