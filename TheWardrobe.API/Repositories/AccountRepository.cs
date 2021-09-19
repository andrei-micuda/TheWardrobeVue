using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TheWardrobe.API.Models;

namespace TheWardrobe.API.Repositories
{
  public class AccountRepository : IAccountRepository
  {
    private readonly IConfiguration _configuration;

    public AccountRepository(IConfiguration configuration)
    {
      this._configuration = configuration;
    }
    public IEnumerable<Account> GetAllAccounts()
    {
      using (var connection = new NpgsqlConnection(_configuration["ConnectionString"]))
      {
        var accounts = connection.Query<Account>("SELECT * FROM accounts");
        return accounts;
      }
    }
  }
}