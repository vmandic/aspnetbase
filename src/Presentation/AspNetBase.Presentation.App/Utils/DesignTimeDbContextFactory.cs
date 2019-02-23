using System.IO;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Common.Utils.Helpers;
using AspNetBase.Core.Settings;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using AspNetBase.Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Presentation.App.Utils
{
  /// <summary>
  /// Adds support for design-time actions such as database migrations.
  /// </summary>
  public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
  {
    public AppDbContext CreateDbContext(string[] args) =>
    new AppDbContext(
      new DbContextOptionsBuilder<AppDbContext>()
      .UseOsDependentDbProvider(ConfigHelper.GetRoot().Bind<DatabaseSettings>("app:database"))
      .Options);
  }

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
