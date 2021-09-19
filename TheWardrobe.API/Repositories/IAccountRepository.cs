using System.Collections.Generic;
using System.Threading.Tasks;
using TheWardrobe.API.Models;

namespace TheWardrobe.API.Repositories
{
  public interface IAccountRepository
  {
    IEnumerable<Account> GetAllAccounts();
  }
}