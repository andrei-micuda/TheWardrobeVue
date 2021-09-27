using System.ComponentModel.DataAnnotations;

namespace TheWardrobe.API.Models.Accounts
{
  public class ValidateResetTokenRequest
  {
    [Required]
    public string Token { get; set; }
  }
}