using AspNetBase.Core.Settings.Attributes;

namespace AspNetBase.Core.Settings
{
  [SettingsKey("app:composition")]
  public class CompositionSettings
  {
    public string[] SkipTypeIfFqnContainsAnyOf { get; set; } = { };
  }
}
