using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Common.Utils.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Composition.Extensions
{
  public static class IServiceCollectionExtensions
  {
    private static IEnumerable<Type> GetIocCoreProviderRegisteredTypes() =>
      typeof(CoreProvidersAssemblyMarker).Assembly
      .GetTypes()
      .Where(x => x.IsDefined(typeof(RegisterDependencyAttribute), false))
      .Distinct();

    public static IServiceCollection RegisterExportedTypes(this IServiceCollection services, ILogger<CompositionRoot> logger)
    {
      var funcType = typeof(Func<>);
      var lazyType = typeof(Lazy<>);

      foreach (var registerType in GetIocCoreProviderRegisteredTypes())
      {
        logger.LogInformation("Processing IoC registration type: {registrationType}", registerType.FullName);

        var dependencyAttr = registerType.GetCustomAttribute<RegisterDependencyAttribute>();
        var contractType = dependencyAttr.InterfaceType ?? registerType.GetInterface($"I{registerType.Name}");

        if (contractType == null)
          throw new InvalidOperationException(
            $"No IoC container registration interface type could be resolved for the concrete type of name '{registerType.FullName}'.");

        switch (dependencyAttr.InjectionStyle)
        {
          case ServiceInjectionStyle.Instant:
            RegisterInstantService(registerType, contractType, dependencyAttr);
            break;
          case ServiceInjectionStyle.Func:
            RegisterFuncService(registerType, contractType, dependencyAttr);
            break;
          case ServiceInjectionStyle.Lazy:
            RegisterLazyService(registerType, contractType, dependencyAttr);
            break;
          case ServiceInjectionStyle.All:
            RegisterInstantService(registerType, contractType, dependencyAttr);
            RegisterFuncService(registerType, contractType, dependencyAttr);
            RegisterLazyService(registerType, contractType, dependencyAttr);
            break;
          default:
            throw new InvalidOperationException("Unsupported IoC container service injection style.");
        }
      }

      return services;

      void RegisterInstantService(
        Type registerType,
        Type contractType,
        RegisterDependencyAttribute depAttr)
      {
        services.Add(new ServiceDescriptor(contractType, registerType, depAttr.Lifetime));
        logger.LogInformation(
          "Type '{type}' injected as '{contract}' with injection style '{style}' and lifetime '{lifetime}'.",
          registerType.FullName,
          contractType.FullName,
          depAttr.InjectionStyle,
          depAttr.Lifetime);
      }

      void RegisterLazyService(
        Type registerType,
        Type contractType,
        RegisterDependencyAttribute depAttr)
      {
        services.Add(new ServiceDescriptor(
          lazyType.MakeGenericType(contractType),

          // WARNING: Generic type unsupported
          sp => Lazy.Create(() => Activator.CreateInstance(registerType)),

          depAttr.Lifetime));

        logger.LogInformation(
          "Type '{type}' injected as '{contract}' with injection style '{style}' and lifetime '{lifetime}'.",
          registerType.FullName,
          contractType.FullName,
          depAttr.InjectionStyle,
          depAttr.Lifetime);
      }

      void RegisterFuncService(
        Type registerType,
        Type contractType,
        RegisterDependencyAttribute depAttr)
      {
        services.Add(new ServiceDescriptor(
          funcType.MakeGenericType(contractType),

          // WARNING: Generic type unsupported
          sp => FuncToObject(() => Activator.CreateInstance(registerType)),

          depAttr.Lifetime));

        logger.LogInformation(
          "Type '{type}' injected as '{contract}' with injection style '{style}' and lifetime '{lifetime}'.",
          registerType.FullName,
          contractType.FullName,
          depAttr.InjectionStyle,
          depAttr.Lifetime);
      }
    }

    private static object FuncToObject<T>(Func<T> func) => func;
  }
}
