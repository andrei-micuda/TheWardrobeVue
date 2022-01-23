using System;

namespace TheWardrobe.CrossCutting.Messages
{
  public class SendEmail
  {
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string HtmlBody { get; set; }
  }
}
