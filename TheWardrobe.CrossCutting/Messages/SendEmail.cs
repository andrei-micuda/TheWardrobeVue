using System;

namespace TheWardrobe.CrossCutting.Messages
{
  public class SendEmail
  {
    public string From { get; } = "andrei.micuda.dev@gmail.com";
    public string To { get; set; }
    public string Subject { get; set; }
    public string HtmlBody { get; set; }
  }
}
