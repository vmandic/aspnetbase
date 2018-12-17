using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AspNetBase.Common.Utils.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetBase.Core.Composition.Extensions
{
  public static class IServiceCollectionExtensions
  {
    private static Type registerDependencyAttributeType =
      typeof(RegisterDependencyAttribute);

    private static IEnumerable<Type> GetIocRegisteredTypes()
    {
      IEnumerable<Type> GetCoreProviderTypes()
      {
        foreach (var type in GetTypesWithIocRegistrationAttribute("AspNetBase.Core.Providers"))
          yield return type;
      }

      return GetCoreProviderTypes().Distinct();
    }

    private static IEnumerable<Type> GetTypesWithIocRegistrationAttribute(string assemblyName) =>
      Assembly
      .Load(assemblyName)
      .GetTypes()
      .Where(x => x.IsDefined(registerDependencyAttributeType, false));

    public static IServiceCollection RegisterExportedTypes(this IServiceCollection services)
    {
      foreach (var registerType in GetIocRegisteredTypes())
      {
        var dependencyAttr = registerType.GetCustomAttribute<RegisterDependencyAttribute>();
        var contractType = registerType.GetInterface($"I{registerType.Name}");

        services.Add(new ServiceDescriptor(contractType, registerType, dependencyAttr.Lifetime));
      }

      return services;
    }
  }
}
