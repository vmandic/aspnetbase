using AspNetBase.Core.Settings.Attributes;

namespace AspNetBase.Core.Settings
{
  [SettingsKey("app")]
  public class AppSettings
  {
    public string Name { get; set; }
    public DatabaseSettings Database { get; set; }
    public ServicesSettings Services { get; set; }
    public LocalizationSettings Localization { get; set; }
  }
}
