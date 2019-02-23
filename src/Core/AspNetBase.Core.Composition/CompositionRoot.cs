using System;
using AspNetBase.Core.Composition.Extensions;
using AspNetBase.Core.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Core.Composition
{
    public abstract class CompositionRoot
  {
    private CompositionRoot() { }

    public static IServiceProvider Initialize(
      IServiceCollection services,
      CompositionSettings compositionSettings,
      ILogger<CompositionRoot> logger)
    {
      if (compositionSettings == null)
        throw new ArgumentNullException(nameof(compositionSettings));

      if (logger == null)
        throw new ArgumentNullException(nameof(logger));

      // NOTE: register exported types
      services.RegisterExportedTypes(compositionSettings, logger);

      // NOTE: other services
      // ...
      // services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, AppUserClaimsPrincipalFactory>();

      return services.BuildServiceProvider();
    }
  }
}
