using System;
using AspNetBase.Core.Composition.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetBase.Core.Composition
{
  public static class CompositionRoot
  {
    public static IServiceProvider Initialize(IServiceCollection services)
    {
      // NOTE: register exported types
      services.RegisterExportedTypes();

      // NOTE: other services

      return services.BuildServiceProvider();
    }
  }
}
