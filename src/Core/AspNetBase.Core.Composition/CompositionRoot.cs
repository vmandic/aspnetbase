using System;
using AspNetBase.Core.Composition.Extensions;
using AspNetBase.Core.Providers.Services.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
      // services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, AppUserClaimsPrincipalFactory>();

      return services.BuildServiceProvider();
    }
  }
}
