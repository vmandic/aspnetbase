using System;
using AspNetBase.Infrastructure.DbInitilizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetBase.Presentation.Server.Extensions
{
  public static class IApplicationBuilderExtensions
  {
    public static IApplicationBuilder MigrateDb(this IApplicationBuilder app)
    {
      var config = app.ApplicationServices.GetService<IConfiguration>();
      var migrateDb = config.GetValue<bool>("Database:MigrateOnStartup");

      if (migrateDb)
        DbInitilizer.Migrate(app.ApplicationServices);

      return app;
    }

    public static IApplicationBuilder SeedDb(this IApplicationBuilder app)
    {
      var config = app.ApplicationServices.GetService<IConfiguration>();
      var seedDb = config.GetValue<bool>("Database:SeedOnStartup");

      if (seedDb)
        DbInitilizer.Seed(app.ApplicationServices);

      return app;
    }
  }
}
