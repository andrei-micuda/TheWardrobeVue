using System.ComponentModel.DataAnnotations;

namespace TheWardrobe.API.Models.Accounts
{
  public class VerifyEmailRequest
  {
    [Required]
    public string Token { get; set; }
  }
}