using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Core.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Composition.Extensions
{
  public static class IServiceCollectionExtensions
  {
    private static IEnumerable<Type> GetIocCoreProviderRegisteredTypes(string[] skipTypeIfFqnContains) =>
      new List<Type[]>
      {
        typeof(CoreProvidersAssemblyMarker).Assembly.GetTypes(),
        typeof(InfrastructureDataAccessAssemblyMarker).Assembly.GetTypes(),
        typeof(InfrastructureDbSeedsAssemblyMarker).Assembly.GetTypes()
      }
      .SelectMany(x => x)
      .Where(x => x.IsDefined(typeof(RegisterDependencyAttribute), false))
      .Where(x => skipTypeIfFqnContains.All(n => !x.FullName.Contains(n)))
      .Distinct(x => x.FullName);

    public static IServiceCollection RegisterExportedTypes(
      this IServiceCollection services,
      CompositionSettings compositionSettings,
      ILogger<CompositionRoot> logger)
    {
      if (services == null)
        throw new ArgumentNullException(nameof(services));

      if (compositionSettings == null)
        throw new ArgumentNullException(nameof(compositionSettings));

      if (logger == null)
        throw new ArgumentNullException(nameof(logger));

      var diStartDateTime = DateTime.Now;

      var targetTypes = GetIocCoreProviderRegisteredTypes(compositionSettings.SkipTypeIfFqnContainsAnyOf).ToList();
      var targetTypeNames = string.Join(Environment.NewLine, targetTypes.Select(x => x.FullName));

      logger.LogInformation(
        "Found ({count}) attribute registered types for IoC DI: {types}",
        targetTypes.Count,
        Environment.NewLine + targetTypeNames);

      // WARNING: parallel processing causes issues
      foreach (var registerType in targetTypes)
      {
        var dependencyAttr = registerType.GetCustomAttribute<RegisterDependencyAttribute>();
        var contractType = dependencyAttr.ContractType ?? registerType.GetInterface($"I{registerType.Name}");

        if (contractType == null)
          throw new InvalidOperationException(
            $"No IoC container registration contract type could be resolved for the concrete type '{registerType.FullName}'.");

        switch (dependencyAttr.InjectionStyle)
        {
          case ServiceInjectionStyle.Instant:
            RegisterInstantService(services, registerType, contractType, dependencyAttr, logger);
            break;
          case ServiceInjectionStyle.Func:
            RegisterFuncService(services, registerType, contractType, dependencyAttr, logger);
            break;
          case ServiceInjectionStyle.Lazy:
            RegisterLazyService(services, registerType, contractType, dependencyAttr, logger);
            break;
          case ServiceInjectionStyle.All:
            RegisterInstantService(services, registerType, contractType, dependencyAttr, logger);
            RegisterFuncService(services, registerType, contractType, dependencyAttr, logger);
            RegisterLazyService(services, registerType, contractType, dependencyAttr, logger);
            break;
          default:
            throw new InvalidOperationException("Unsupported IoC container service injection style.");
        }
      }

      logger.LogInformation("Dependency injection finished in: '{elapsed}'", DateTime.Now - diStartDateTime);
      return services;
    }

    static void RegisterInstantService(
      IServiceCollection services,
      Type registerType,
      Type contractType,
      RegisterDependencyAttribute depAttr,
      ILogger logger)
    {
      services.Add(new ServiceDescriptor(contractType, registerType, depAttr.Lifetime));
      LogInjection(logger, registerType, contractType, depAttr);
    }

    static void RegisterLazyService(
      IServiceCollection services,
      Type registerType,
      Type contractType,
      RegisterDependencyAttribute depAttr,
      ILogger logger)
    {
      services.Add(new ServiceDescriptor(
        typeof(Lazy<>).MakeGenericType(contractType),

        // WARNING: Generic type unsupported
        sp => Lazy.Create(() => Activator.CreateInstance(registerType)),

        depAttr.Lifetime));

      LogInjection(logger, registerType, contractType, depAttr);
    }

    private static void RegisterFuncService(
      IServiceCollection services,
      Type registerType,
      Type contractType,
      RegisterDependencyAttribute depAttr,
      ILogger logger)
    {
      services.Add(new ServiceDescriptor(
        typeof(Func<>).MakeGenericType(contractType),

        // WARNING: Generic type unsupported
        sp => FuncToObject(() => Activator.CreateInstance(registerType)),

        depAttr.Lifetime));

      LogInjection(logger, registerType, contractType, depAttr);
    }

    private static void LogInjection(
      ILogger logger,
      Type registerType,
      Type contractType,
      RegisterDependencyAttribute depAttr)
    {
      logger.LogInformation(
        "Type '{type}' injected as '{contract}' with style '{style}' and scope '{lifetime}'.",
        registerType.FullName,
        contractType.FullName,
        depAttr.InjectionStyle,
        depAttr.Lifetime);
    }

    private static object FuncToObject<T>(Func<T> func) => func;
  }
}
