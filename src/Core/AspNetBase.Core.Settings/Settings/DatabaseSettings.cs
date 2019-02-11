using System.Runtime.InteropServices;
using AspNetBase.Core.Settings.Attributes;

namespace AspNetBase.Core.Settings
{
  [SettingsKey("app:database")]
  public class DatabaseSettings
  {
    public bool AlwaysUseSqlite { get; set; }
    public bool MigrateOnStartup { get; set; }
    public bool SeedOnStartup { get; set; }

    public ConnectionStringsSettings ConnectionStrings { get; set; }

    public string GetConnectionString() =>
      ShouldUseSqlServer()
        ? ConnectionStrings.MsSqlDev
        : ConnectionStrings.SqliteDev;

    public bool ShouldUseSqlServer() =>
      RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !AlwaysUseSqlite;
  }
}
