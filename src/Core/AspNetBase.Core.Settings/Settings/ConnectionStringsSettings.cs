using AspNetBase.Core.Settings.Attributes;

namespace AspNetBase.Core.Settings
{
  [SettingsKey("app:database:connectionstrings")]
  public class ConnectionStringsSettings
  {
    public string MsSqlDev { get; set; }
    public string SqliteDev { get; set; }
  }
}
