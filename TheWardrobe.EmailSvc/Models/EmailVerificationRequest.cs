namespace TheWardrobe.EmailSvc.Models
{
  public class EmailVerificationRequest
  {
    public string ToEmail { get; set; }
    public string VerificationUrl { get; set; }
  }
}