using AspNetBase.Core.Settings.Base;

namespace AspNetBase.Core.Settings
{
  public class DatabaseSettings : ISetting
  {
    public bool AlwaysUseSqlite { get; set; }
    public bool MigrateOnStartup { get; set; }
    public bool SeedOnStartup { get; set; }

    public string Key => "app:database";
  }
}
