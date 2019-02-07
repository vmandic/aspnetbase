using AspNetBase.Core.Settings.Attributes;

namespace AspNetBase.Core.Settings
{
  [SettingsKey("app:database")]
  public class DatabaseSettings
  {
    public bool AlwaysUseSqlite { get; set; }
    public bool MigrateOnStartup { get; set; }
    public bool SeedOnStartup { get; set; }
  }
}
