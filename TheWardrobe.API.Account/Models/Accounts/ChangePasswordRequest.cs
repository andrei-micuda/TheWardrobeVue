using System.ComponentModel.DataAnnotations;

namespace TheWardrobe.API.Models.Accounts
{
  public class ChangePasswordRequest
  {
    [Required]
    public string Token { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
  }
}