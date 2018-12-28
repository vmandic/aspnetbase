using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Common.Utils.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspNetBase.Core.Composition.Extensions
{
  public static class IServiceCollectionExtensions
  {
    private static readonly Type registerDependencyAttributeType =
      typeof(RegisterDependencyAttribute);

    private static IEnumerable<Type> GetIocCoreProviderRegisteredTypes() =>
      Assembly
        .Load("AspNetBase.Core.Providers")
        .GetTypes()
        .Where(x => x.IsDefined(registerDependencyAttributeType, false))
        .Distinct();

    public static IServiceCollection RegisterExportedTypes(this IServiceCollection services)
    {
      var funcType = typeof(Func<>);
      var lazyType = typeof(Lazy<>);

      foreach (var registerType in GetIocCoreProviderRegisteredTypes())
      {
        var dependencyAttr = registerType.GetCustomAttribute<RegisterDependencyAttribute>();
        var contractType = dependencyAttr.InterfaceType ?? registerType.GetInterface($"I{registerType.Name}");

        if (contractType == null)
          throw new InvalidOperationException($"No IoC interface type could be resolved for a concrete type '{registerType.FullName}'.");

        switch (dependencyAttr.InjectionStyle)
        {
          case ServiceInjectionStyle.Instant:
            RegisterInstantService(services, registerType, dependencyAttr, contractType);
            break;
          case ServiceInjectionStyle.Func:
            RegisterFuncService(services, funcType, registerType, dependencyAttr, contractType);
            break;
          case ServiceInjectionStyle.Lazy:
            RegisterLazyService(services, lazyType, registerType, dependencyAttr, contractType);
            break;
          case ServiceInjectionStyle.All:
            RegisterInstantService(services, registerType, dependencyAttr, contractType);
            RegisterFuncService(services, funcType, registerType, dependencyAttr, contractType);
            RegisterLazyService(services, lazyType, registerType, dependencyAttr, contractType);
            break;
          default:
            throw new InvalidOperationException("Unsupported service injection style.");
        }
      }

      return services;
    }

    private static void RegisterInstantService(
      IServiceCollection services,
      Type registerType,
      RegisterDependencyAttribute dependencyAttr,
      Type contractType)
    {
      services.Add(new ServiceDescriptor(contractType, registerType, dependencyAttr.Lifetime));
    }

    private static void RegisterLazyService(
      IServiceCollection services,
      Type lazyType,
      Type registerType,
      RegisterDependencyAttribute dependencyAttr,
      Type contractType)
    {
      services.Add(new ServiceDescriptor(
        lazyType.MakeGenericType(contractType),

        // WARNING: Generic type unsupported
        sp => Lazy.Create(() => Activator.CreateInstance(registerType)),

        dependencyAttr.Lifetime));
    }

    private static void RegisterFuncService(
      IServiceCollection services,
      Type funcType,
      Type registerType,
      RegisterDependencyAttribute dependencyAttr,
      Type contractType)
    {
      services.Add(new ServiceDescriptor(
        funcType.MakeGenericType(contractType),

        // WARNING: Generic type unsupported
        sp => FuncToObject(() => Activator.CreateInstance(registerType)),

        dependencyAttr.Lifetime));
    }

    private static object FuncToObject<T>(Func<T> func) => func;
  }
}
