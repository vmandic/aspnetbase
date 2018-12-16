using System.IO;
using System.Runtime;
using AspNetBase.Infrastructure.DataAccess.Data;
using AspNetBase.Presentation.Server.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Presentation.Server.Utilities
{
  public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
  {
    public AppDbContext CreateDbContext(string[] args) =>
      new AppDbContext(
        new DbContextOptionsBuilder<AppDbContext>()
          .UseOsDependentDbProvider(ConfigHelper.GetRoot())
          .Options);
  }
}
