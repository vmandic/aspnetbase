using System;
using System.Linq;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Core.Settings.Attributes;
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

      var settingsKeyAttributeType = typeof(SettingsKeyAttribute);
      var settingTypes = settingsKeyAttributeType.Assembly
        .GetTypes()
        .Where(x => x.IsDefined(settingsKeyAttributeType, false))
        .ToList();

      if (settingTypes.Count == 0)
        throw new InvalidOperationException("No application settings were found to be configured and registerd in the IoC container.");

      void ConfigureSetting<TSetting>(TSetting metaSettingsType) where TSetting : class
      {
        var settingsType = metaSettingsType as Type;
        var settingsKeyAttr = (SettingsKeyAttribute) settingsType
          .GetCustomAttributes(settingsKeyAttributeType, false).Single();

        var settingsInstance = config.Bind(settingsType, settingsKeyAttr.KeyName);

        if (settingsInstance == null)
          throw new InvalidOperationException($"Setting '{settingsType.FullName}' is null after trying to bind from config.");

        services.Add(new ServiceDescriptor(settingsType, settingsInstance));
      }

      settingTypes.ForEach(ConfigureSetting);

      return services;
    }
  }
}
