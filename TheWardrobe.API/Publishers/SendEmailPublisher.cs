using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using TheWardrobe.CrossCutting.Messages;

namespace TheWardrobe.API.Publishers
{
  public interface ISendEmailPublisher
  {
    Task SendVerificationEmail(string verificationToken);
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

    public async Task SendVerificationEmail(string verificationToken)
    {
      var emailVerificationTemplate = _config.GetSection("EmailTemplates:EmailVerificationTemplate");

      await _bus.Send(new SendEmail
      {
        From = "gicu@yahoo.com",
        To = "test@yahoo.com",
        Subject = emailVerificationTemplate["Subject"],
        HtmlBody = emailVerificationTemplate["HtmlBody"]
      });
    }
  }
}