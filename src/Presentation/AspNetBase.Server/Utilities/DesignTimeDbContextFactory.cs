using System.IO;
using System.Runtime;
using AspNetBase.DataAccess.Data;
using AspNetBase.Server.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Server.Utilities
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
