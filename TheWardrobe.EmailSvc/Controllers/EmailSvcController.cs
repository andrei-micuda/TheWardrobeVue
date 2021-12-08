using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TheWardrobe.EmailSvc.Models;

namespace TheWardrobe.EmailSvc.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class EmailSvcController : ControllerBase
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailSvcController> _logger;
    private readonly IEmailService _emailService;

    public EmailSvcController(IConfiguration configuration, ILogger<EmailSvcController> logger, IEmailService emailService)
    {
      _configuration = configuration;
      _logger = logger;
      _emailService = emailService;
    }


    [HttpPost]
    [Route("sendVerificationEmail")]
    public void SendVerificationEmail(EmailVerificationRequest model)
    {
      var emailVerificationTemplate = _configuration.GetSection("EmailVerificationTemplate");
      _emailService.Send(
        "andrei.micuda.dev@gmail.com",
        model.ToEmail,
        emailVerificationTemplate["Subject"],
        @$"
        {emailVerificationTemplate["HtmlBody"]}
        <a href={model.VerificationUrl} target='_blank'>{model.VerificationUrl}</a>
        ");
    }
  }
}
