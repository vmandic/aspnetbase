using AspNetBase.Core.Settings.Base;

namespace AspNetBase.Core.Settings
{
  public class ServicesSettings : ISetting
  {
    public EmailSenderSettings EmailSender { get; set; }
    public string Key => "app:services";
  }
}
