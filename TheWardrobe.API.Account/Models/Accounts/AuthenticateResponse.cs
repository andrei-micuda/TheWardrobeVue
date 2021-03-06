using System;
using System.Text.Json.Serialization;

namespace TheWardrobe.API.Models.Accounts
{
  public class AuthenticateResponse
  {
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime WhenCreated { get; set; }
    public DateTime? WhenUpdated { get; set; }
    public bool IsVerified { get; set; }
    public string Jwt { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }
  }
}