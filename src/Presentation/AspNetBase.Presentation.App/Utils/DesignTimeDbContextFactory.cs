using AspNetBase.Common.Utils.Extensions;
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
}
