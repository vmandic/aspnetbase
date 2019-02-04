using AspNetBase.Common.Utils.Attributes;

namespace AspNetBase.Presentation.App.Settings
{
  [Settings("app:services:emailsender")]
  public class EmailSenderSettings
  {
    public bool Enabled { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string From { get; set; }
    public string Sender { get; set; }
    public string Password { get; set; }
  }
}
