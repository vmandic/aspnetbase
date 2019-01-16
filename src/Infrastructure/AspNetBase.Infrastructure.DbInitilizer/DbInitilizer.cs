using System;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using AspNetBase.Infrastructure.DbInitilizer.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetBase.Infrastructure.DbInitilizer
{
  public static class DbInitilizer
  {
    /// <summary>
    /// Runs database code first migrations if any are pending.
    /// </summary>
    public static void Migrate(IServiceProvider serviceProvider)
    {
      if (serviceProvider == null)
        throw new ArgumentNullException(nameof(serviceProvider));

      using(var scope = serviceProvider.CreateScope())
      {
        var scopedServiceProvider = scope.ServiceProvider;
        var db = scopedServiceProvider.GetService<AppDbContext>();

        db.Database.Migrate();
      }
    }

    /// <summary>
    /// Runs a custom data seed procedure to ensure that specific data always exists in the database.
    /// </summary>
    public static void Seed(IServiceProvider serviceProvider)
    {
      if (serviceProvider == null)
        throw new ArgumentNullException(nameof(serviceProvider));

      using(var scope = serviceProvider.CreateScope())
      {
        var services = scope.ServiceProvider;

        var dbContext = services.GetService<AppDbContext>();
        var userManager = services.GetService<UserManager<AppUser>>();

        // Custom seed procedures per entity:
        new AppUserSeed(dbContext, userManager).Run();
      }
    }
  }
}
