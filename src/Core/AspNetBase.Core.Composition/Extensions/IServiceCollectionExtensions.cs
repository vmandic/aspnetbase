using AspNetBase.Common.Utils.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AspNetBase.Core.Composition.Extensions
{
  public static class IServiceCollectionExtensions
  {
    private static readonly Lazy<IEnumerable<Type>> TypesToRegister =
      new Lazy<IEnumerable<Type>>(() => GetIocRegisteredTypes());

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
      foreach (var registerType in TypesToRegister.Value)
      {
        var dep = registerType.GetCustomAttribute<RegisterDependencyAttribute>();
        services.Add(new ServiceDescriptor(dep.ContractType, registerType, dep.Lifetime));
      }

      return services;
    }
  }
}
