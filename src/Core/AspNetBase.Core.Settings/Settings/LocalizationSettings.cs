using AspNetBase.Core.Settings.Attributes;

namespace AspNetBase.Core.Settings
{
  [SettingsKey("app:localization")]
  public class LocalizationSettings
  {
    public string DefaultCulture { get; set; } = "en";
    public string LocalizationCookieName { get; set; } = ".AspNetCore.AspNetBase.Culture";
    public string[] SupportedCultures { get; set; } = { "en" };
  }
}
