using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Infrastructure.Settings;

namespace AspNetBase.Presentation.App.Settings
{
  [Settings("app")]
  public class AppSettings
  {
    public DatabaseSettings Database { get; set; }
    public ServicesSettings Services { get; set; }

  }
}
