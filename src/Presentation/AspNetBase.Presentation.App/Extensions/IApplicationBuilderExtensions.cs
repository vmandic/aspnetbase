using System;
using AspNetBase.Core.Settings;
using AspNetBase.Infrastructure.DbInitilizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetBase.Presentation.App.Extensions
{
  public static class IApplicationBuilderExtensions
  {
    public static IApplicationBuilder MigrateDb(this IApplicationBuilder app, DatabaseSettings dbSettings)
    {
      if (dbSettings == null)
        throw new ArgumentNullException(nameof(dbSettings));

      if (dbSettings.MigrateOnStartup)
        DbInitilizer.Migrate(app.ApplicationServices);

      return app;
    }

    public static IApplicationBuilder SeedDb(this IApplicationBuilder app, DatabaseSettings dbSettings)
    {
      if (dbSettings == null)
        throw new ArgumentNullException(nameof(dbSettings));

      if (dbSettings.SeedOnStartup)
        DbInitilizer.Seed(app.ApplicationServices);

      return app;
    }
  }
}
