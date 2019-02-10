using System;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Common.Utils.Extensions
{
  public static class IConfigurationExtensions
  {
    public static TSetting Bind<TSetting>(this IConfiguration config, string key) where TSetting : class
    {
      if (config == null)
        throw new ArgumentNullException(nameof(config));

      if (string.IsNullOrWhiteSpace(key))
        throw new ArgumentException("Invalid config key value provided.", nameof(key));

      var settingType = typeof(TSetting);
      var setting = Activator.CreateInstance<TSetting>();

      config.Bind(key, setting);

      return setting;
    }

    public static object Bind<TSetting>(this IConfiguration config, TSetting settingType, string key) where TSetting : class
    {
      if (config == null)
        throw new ArgumentNullException(nameof(config));

      if (settingType == null)
        throw new ArgumentNullException(nameof(settingType));

      if (string.IsNullOrWhiteSpace(key))
        throw new ArgumentException("Invalid config key value provided.", nameof(key));

      var setting = Activator.CreateInstance(settingType as Type);
      config.Bind(key, setting);

      return setting;
    }
  }
}
