using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWardrobe.API.Models.Accounts
{
  public class VerifyAccountRequest
  {
    public string VerificationToken { get; set; }
  }
}