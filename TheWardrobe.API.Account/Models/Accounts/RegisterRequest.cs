using System.ComponentModel.DataAnnotations;
using TheWardrobe.API;

namespace TheWardrobe.API.Models.Accounts
{
  public class RegisterRequest
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
  }
}