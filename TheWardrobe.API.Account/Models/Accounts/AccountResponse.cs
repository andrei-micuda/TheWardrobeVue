using System;

namespace TheWardrobe.API.Models.Accounts
{
  public class AccountResponse
  {
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime WhenCreated { get; set; }
    public DateTime? WhenUpdated { get; set; }
    public bool IsVerified { get; set; }
  }
}