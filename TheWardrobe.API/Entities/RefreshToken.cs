using System;
using System.ComponentModel.DataAnnotations;

namespace TheWardrobe.API.Entities
{
  public class RefreshToken
  {
    [Key]
    public int Id { get; set; }
    public Account Account { get; set; }
    public int AccountId { get; set; }
    public string Token { get; set; }
    public DateTime WhenExpires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= WhenExpires;
    public DateTime WhenCreated { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? WhenRevoked { get; set; }
    public string RevokedByIp { get; set; }
    public string ReplacedByToken { get; set; }
    public bool IsActive => WhenRevoked == null && !IsExpired;
  }
}