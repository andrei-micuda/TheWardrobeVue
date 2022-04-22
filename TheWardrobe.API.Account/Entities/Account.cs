using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TheWardrobe.API.Entities
{
  [Table("account")]
  public class Account
  {
    [Column("id")]
    public Guid Id { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [JsonIgnore]
    [Column("password_hash")]
    public string PasswordHash { get; set; }

    [Editable(false)]
    public Role Role { get; set; }

    [Column("verification_token")]
    public string VerificationToken { get; set; }

    [Column("when_verified")]
    public DateTime? WhenVerified { get; set; }

    [Editable(false)]
    public bool IsVerified => WhenVerified.HasValue || WhenPasswordReset.HasValue;

    [Column("reset_token")]
    public string ResetToken { get; set; }

    [Column("when_reset_token_expires")]
    public DateTime? WhenResetTokenExpires { get; set; }

    [Column("when_password_reset")]
    public DateTime? WhenPasswordReset { get; set; }

    [Column("when_created")]
    public DateTime WhenCreated { get; set; }

    [Column("when_updated")]
    public DateTime? WhenUpdated { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; }

    public bool OwnsToken(string token)
    {
      return this.RefreshTokens?.Find(x => x.Token == token) != null;
    }
  }
}