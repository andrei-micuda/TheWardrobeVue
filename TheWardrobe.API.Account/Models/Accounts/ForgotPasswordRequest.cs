using System.ComponentModel.DataAnnotations;

namespace TheWardrobe.API.Models.Accounts
{
  public class ForgotPasswordRequest
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }
  }
}