using System.IO;
using AspNetBase.Infrastructure.DataAccess.Data;
using AspNetBase.Infrastructure.DataAccess.Extensions;
using AspNetBase.Presentation.Server.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Presentation.Server.Utilities
{
  // Adds support for design-time actions such as database migrations.
  public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
  {
    public AppDbContext CreateDbContext(string[] args) =>
    new AppDbContext(
      new DbContextOptionsBuilder<AppDbContext>()
      .UseOsDependentDbProvider(ConfigHelper.GetRoot())
      .Options);

    class ConfigHelper
    {
      static readonly object locker = new object();
      static IConfigurationRoot root;

      public static IConfigurationRoot GetRoot()
      {
        lock(locker)
        {
          return root ?? (root = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build());
        }
      }
    }
  }
}
