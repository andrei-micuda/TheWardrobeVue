using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Logging;
using TheWardrobe.CrossCutting.Messages;

namespace TheWardrobe.EmailSvc.Consumers
{
  public class SendEmailConsumer :
      IConsumer<SendEmail>
  {
    readonly ILogger<SendEmailConsumer> _logger;
    readonly IEmailService _emailService;

    public SendEmailConsumer(ILogger<SendEmailConsumer> logger, IEmailService emailService)
    {
      _logger = logger;
      _emailService = emailService;
    }

    public Task Consume(ConsumeContext<SendEmail> context)
    {
      _logger.LogInformation("Received Text: {Text}", context.Message.Subject);
      var sendEmail = context.Message;

      _emailService.Send(
        sendEmail.From,
        sendEmail.To,
        sendEmail.Subject,
        sendEmail.HtmlBody);
      // emailVerificationTemplate["Subject"],
      // @$"
      // {emailVerificationTemplate["HtmlBody"]}
      // <a href={model.VerificationUrl} target='_blank'>{model.VerificationUrl}</a>
      // ");

      return Task.CompletedTask;
    }
  }
}