using AspNetBase.Core.Composition.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace AspNetBase.Core.Composition
{
  public abstract class CompositionRoot
  {
    private CompositionRoot() { }

    public static IServiceProvider Initialize(
      IServiceCollection services,
      ILogger<CompositionRoot> logger)
    {
      // NOTE: register exported types
      services.RegisterExportedTypes(logger);


      // NOTE: other services
      // ...

      return services.BuildServiceProvider();
    }
  }
}
