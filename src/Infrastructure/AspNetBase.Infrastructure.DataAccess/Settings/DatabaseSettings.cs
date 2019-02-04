using AspNetBase.Common.Utils.Attributes;

namespace AspNetBase.Infrastructure.Settings
{
  [Settings("app:database")]
  public class DatabaseSettings
  {
    public bool AlwaysUseSqlite { get; set; }
    public bool MigrateOnStartup { get; set; }
    public bool SeedOnStartup { get; set; }
  }
}
