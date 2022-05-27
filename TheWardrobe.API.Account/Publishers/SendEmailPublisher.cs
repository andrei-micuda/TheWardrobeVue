using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using TheWardrobe.CrossCutting.Messages;

namespace TheWardrobe.API.Publishers
{
  public interface ISendEmailPublisher
  {
    Task SendVerificationEmail(string accountEmail, string verificationToken);
    Task SendForgotPasswordEmail(string accountEmail, string resetToken);
  }
  public class SendEmailPublisher : ISendEmailPublisher
  {
    private readonly IBus _bus;
    private readonly IConfiguration _config;
    public SendEmailPublisher(IConfiguration config, IBus bus)
    {
      _bus = bus;
      _config = config;
    }

    public async Task SendForgotPasswordEmail(string accountEmail, string resetToken)
    {
      var resetUrl = $"http://localhost:8080/#/change-password?token={resetToken}";

      await _bus.Send(new SendEmail
      {
        To = accountEmail,
        Subject = "TheWardrobe -  Reset Password",
        HtmlBody = @$"
        <p>Click the following link in order to reset your TheWardrobe password:</p>
        </br>
        <a href='{resetUrl}'>{resetUrl}</a>
        <br>
        <p>If you did not initiate this action, plase ignore this message.</p>
        "
      });
    }

    public async Task SendVerificationEmail(string accountEmail, string verificationToken)
    {
      var emailVerificationTemplate = _config.GetSection("EmailTemplates:EmailVerificationTemplate");

      var verificationUrl = $"http://localhost:8080/#/verify-account?token={verificationToken}";

      await _bus.Send(new SendEmail
      {
        To = accountEmail,
        Subject = emailVerificationTemplate["Subject"],
        HtmlBody = @$"
        {emailVerificationTemplate["HtmlBody"]}
        <a href='{verificationUrl}'>{verificationUrl}</a>
        "
      });
    }
  }
}