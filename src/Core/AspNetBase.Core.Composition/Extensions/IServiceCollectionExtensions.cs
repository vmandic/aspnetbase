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
    private static IEnumerable<Type> GetIocRegisteredTypes()
    {
      IEnumerable<Type> GetTypes()
      {
        foreach (var assemblyName in GetReferencedAssemblyNames())
          foreach (var type in GetTypesWithIocRegistrationAttribute(assemblyName))
            yield return type;
      }

      return GetTypes().Distinct();
    }

    private static IEnumerable<Type> GetTypesWithIocRegistrationAttribute(AssemblyName assemblyName) =>
      Assembly
        .Load(assemblyName)
        .GetTypes()
        .Where(x => x.GetCustomAttribute<RegisterDependencyAttribute>() != null);

    private static AssemblyName[] GetReferencedAssemblyNames() =>
      Assembly
        .GetExecutingAssembly()
        .GetReferencedAssemblies();

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
