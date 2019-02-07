using AspNetBase.Core.Settings.Attributes;

namespace AspNetBase.Core.Settings
{
  [SettingsKey("app")]
  public class AppSettings
  {
    public DatabaseSettings Database { get; set; }
    public ServicesSettings Services { get; set; }
  }
}
