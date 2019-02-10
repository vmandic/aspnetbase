using System;
using System.Linq;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Core.Settings.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Settings.Extensions
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddSettings(
      this IServiceCollection services,
      IConfiguration config,
      ILogger<SettingsRegistration> logger)
    {
      if (services == null)
        throw new ArgumentNullException(nameof(services));

      if (config == null)
        throw new ArgumentNullException(nameof(config));

      if (logger == null)
        throw new ArgumentNullException(nameof(logger));

      var settingsDiStartDateTime = DateTime.Now;

      var settingsKeyAttributeType = typeof(SettingsKeyAttribute);
      var settingTypes = settingsKeyAttributeType.Assembly
        .GetTypes()
        .Where(x => x.IsDefined(settingsKeyAttributeType, false))
        .ToList();

      if (settingTypes.Count == 0)
        throw new InvalidOperationException("No application settings were found to be configured and registerd in the IoC container.");

      var foundTypeNames = Environment.NewLine +
        string.Join(Environment.NewLine, settingTypes.Select(x => x.FullName));

      logger.LogInformation(
        "Found settings ({count}) for IoC injection: {types}",
        settingTypes.Count,
        foundTypeNames);

      void ConfigureSetting<TSetting>(TSetting metaSettingsType) where TSetting : class
      {
        var settingsType = metaSettingsType as Type;
        var settingsKeyAttr = (SettingsKeyAttribute) settingsType
          .GetCustomAttributes(settingsKeyAttributeType, false).Single();

        var settingsInstance = config.Bind(settingsType, settingsKeyAttr.KeyName);

        if (settingsInstance == null)
          throw new InvalidOperationException($"Setting '{settingsType.FullName}' is null after trying to bind from config.");

        services.Add(new ServiceDescriptor(settingsType, settingsInstance));
        SettingsLocator.Set(settingsType, settingsInstance);

        logger.LogInformation(
          "Injected setting '{settingTypeFn}' with value: {setting}",
          settingsType.FullName,
          Environment.NewLine + settingsInstance.ToJson());
      }

      settingTypes.ForEach(ConfigureSetting);

      logger.LogInformation("Settings injection finished in: '{elapsed}'", DateTime.Now - settingsDiStartDateTime);
      return services;
    }
  }
}
