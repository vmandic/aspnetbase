using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AspNetBase.DataAccess.Data;
using System.Runtime.InteropServices;

namespace AspNetBase.Server.Utilities
{
  public static class ConnectionStringHelper
  {
    static bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static string GetOsDependentConnectionString(IConfigurationRoot config) =>
      config.GetConnectionString(isWindows ? "MsSqlLocalDb" : "SqlLiteLocalDb");

    public static DbContextOptionsBuilder<AppDbContext> UseOsDependentDbProvider(
      this DbContextOptionsBuilder<AppDbContext> builder,
      IConfigurationRoot config)
    {
      var connectionString = GetOsDependentConnectionString(config);

      return isWindows ?
        builder.UseSqlServer(connectionString) :
        builder.UseSqlite(connectionString);
    }
  }
}
