using System.Runtime.InteropServices;
using AspNetBase.Infrastructure.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Presentation.Server.Utilities
{
  public static class ConnectionStringHelper
  {
    static bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static string GetOsDependentConnectionString(IConfigurationRoot config) =>
      config.GetConnectionString(!config.GetValue<bool>("AlwaysUseSqlite") && isWindows ?
        "MsSqlLocalDb" :
        "SqlLiteLocalDb");

    public static DbContextOptionsBuilder<AppDbContext> UseOsDependentDbProvider(
      this DbContextOptionsBuilder<AppDbContext> builder,
      IConfigurationRoot config)
    {
      var connectionString = GetOsDependentConnectionString(config);
      var forceSqlite = config.GetValue<bool>("AlwaysUseSqlite");

      string dbCtxAssemblyNamespace = typeof(AppDbContext).Assembly.GetName().Name;

      return !forceSqlite && isWindows ?
        builder.UseSqlServer(connectionString, opts => opts.MigrationsAssembly(dbCtxAssemblyNamespace)) :
        builder.UseSqlite(connectionString, opts => opts.MigrationsAssembly(dbCtxAssemblyNamespace));
    }
  }
}
