using System.IO;
using System.Runtime;
using AspNetBase.Infrastructure.DataAccess.Data;
using AspNetBase.Presentation.Server.Utilities;
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
  }
}
