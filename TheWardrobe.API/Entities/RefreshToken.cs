using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheWardrobe.API.Entities
{
  [Table("refresh_token")]
  public class RefreshToken
  {
    [Column("id")]
    public Guid Id { get; set; }

    public Account Account { get; set; }

    [Column("account_id")]
    public Guid AccountId { get; set; }

    [Column("token")]
    public string Token { get; set; }

    [Column("when_expires")]
    public DateTime WhenExpires { get; set; }

    [Editable(false)]
    public bool IsExpired => DateTime.UtcNow >= WhenExpires;

    [Column("when_created")]
    public DateTime WhenCreated { get; set; }

    [Column("created_by_ip")]
    public string CreatedByIp { get; set; }

    [Column("when_revoked")]
    public DateTime? WhenRevoked { get; set; }

    [Column("revoked_by_ip")]
    public string RevokedByIp { get; set; }

    [Column("replaced_by_token")]
    public string ReplacedByToken { get; set; }

    [Editable(false)]
    public bool IsActive => WhenRevoked == null && !IsExpired;
  }
}