using System.ComponentModel.DataAnnotations;
using TheWardrobe.API;

namespace TheWardrobe.API.Models.Accounts
{
  public class RegisterRequest
  {
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public string Role { get; set; } = Entities.Role.User.ToString();
  }
}