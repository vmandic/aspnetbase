using System;
using System.Linq;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Core.Settings.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetBase.Core.Settings.Extensions
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration config)
    {
      if (services == null)
      {
        throw new ArgumentNullException(nameof(services));
      }

      if (config == null)
      {
        throw new ArgumentNullException(nameof(config));
      }

      var iSettingType = typeof(ISetting);
      var settingTypes = iSettingType.Assembly
        .GetTypes()
        .Where(x => x.GetInterfaces().Contains(iSettingType))
        .ToList();

      if (settingTypes.Count == 0)
        throw new InvalidOperationException("No application settings were found to be configured and registerd in the IoC container.");

      void ConfigureSetting<TSetting>(TSetting metaSettingsType) where TSetting : class
      {
        Type settingsType = metaSettingsType as Type;
        var settingsIntf = metaSettingsType as ISetting;

        var settingsInstance = config.Bind(settingsType, settingsIntf.Key);

        if (settingsInstance == null)
          throw new InvalidOperationException($"Setting '{settingsType.FullName}' is null after trying to bind from config.");

        services.AddSingleton(settingsInstance);
      }

      settingTypes.ForEach(ConfigureSetting);

      return services;
    }
  }
}
