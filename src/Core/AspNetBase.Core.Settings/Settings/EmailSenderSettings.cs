using AspNetBase.Core.Settings.Base;

namespace AspNetBase.Core.Settings
{
  public class EmailSenderSettings : ISetting
  {
    public bool Enabled { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string From { get; set; }
    public string Sender { get; set; }
    public string Password { get; set; }
    public string Key => "app:services:emailsender";
  }
}
