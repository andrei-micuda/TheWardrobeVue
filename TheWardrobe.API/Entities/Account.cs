using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TheWardrobe.API.Entities
{
  public class Account
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    [JsonIgnore]
    public string PasswordHash { get; set; }
    public Role Role { get; set; }
    public string VerificationToken { get; set; }
    public DateTime? WhenVerified { get; set; }
    public bool IsVerified => WhenVerified.HasValue || WhenPasswordReset.HasValue;
    public string ResetToken { get; set; }
    public DateTime? WhenResetTokenExpires { get; set; }
    public DateTime? WhenPasswordReset { get; set; }
    public DateTime WhenCreated { get; set; }
    public DateTime? WhenUpdated { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }

    public bool OwnsToken(string token)
    {
      return this.RefreshTokens?.Find(x => x.Token == token) != null;
    }
  }
}