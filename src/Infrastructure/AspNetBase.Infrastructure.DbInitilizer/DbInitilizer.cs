using System;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetBase.Infrastructure.DbInitilizer
{
  public static class DbInitilizer
  {
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

    public static void Seed(IServiceProvider serviceProvider)
    {
      if (serviceProvider == null)
        throw new ArgumentNullException(nameof(serviceProvider));

      using(var scope = serviceProvider.CreateScope())
      {
        var scopedServiceProvider = scope.ServiceProvider;
        var db = scopedServiceProvider.GetService<AppDbContext>();

        // TODO: implement custom seed, look for AddOrUpdate method...
      }
    }
  }
}
