using AspNetBase.Core.Settings.Base;

namespace AspNetBase.Core.Settings
{
  public class AppSettings : ISetting
  {
    public string Key => "app";
    public DatabaseSettings Database { get; set; }
    public ServicesSettings Services { get; set; }
  }
}
