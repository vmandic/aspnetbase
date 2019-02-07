using AspNetBase.Core.Settings.Attributes;

namespace AspNetBase.Core.Settings
{
  [SettingsKey("app:services")]
  public class ServicesSettings
  {
    public EmailSenderSettings EmailSender { get; set; }
  }
}
