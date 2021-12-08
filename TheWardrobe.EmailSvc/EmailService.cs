using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System;
using MimeKit.Text;

namespace TheWardrobe.EmailSvc
{
  public interface IEmailService
  {
    void Send(string from, string to, string subject, string html);
  }

  public class EmailService : IEmailService
  {
    private readonly IConfiguration _smtpConfiguration;

    public EmailService(IConfiguration configuration)
    {
      _smtpConfiguration = configuration.GetSection("Smtp");
    }

    public void Send(string from, string to, string subject, string html)
    {
      // create message
      var email = new MimeMessage();
      email.From.Add(MailboxAddress.Parse(from));
      email.To.Add(MailboxAddress.Parse(to));
      email.Subject = subject;
      email.Body = new TextPart(TextFormat.Html) { Text = html };

      // send email
      using var smtp = new SmtpClient();
      smtp.Connect(_smtpConfiguration["Host"], Int32.Parse(_smtpConfiguration["Port"]), SecureSocketOptions.StartTls);
      smtp.Authenticate(_smtpConfiguration["Username"], _smtpConfiguration["Password"]);
      smtp.Send(email);
      smtp.Disconnect(true);
    }
  }
}