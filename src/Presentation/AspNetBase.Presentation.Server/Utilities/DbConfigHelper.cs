using System.Runtime.InteropServices;
using AspNetBase.Infrastructure.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Presentation.Server.Utilities
{
  public static class DbConfigHelper
  {
    public static bool IsCurrentOsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    private static string GetOsDependentConnectionString(IConfiguration config, bool alwaysUseSqlite) =>
      config.GetConnectionString(!alwaysUseSqlite && IsCurrentOsWindows ?
        "MsSqlLocalDb" :
        "SqliteLocalDb");

    public static(string conString, bool forceSqlite, string migrationsAssembly) GetDbProviderDetails(IConfiguration config)
    {
      var alwaysUseSqlite = config.GetValue<bool>("AlwaysUseSqlite");

      return (
        GetOsDependentConnectionString(config, alwaysUseSqlite),
        alwaysUseSqlite,
        typeof(AppDbContext).Assembly.GetName().Name);
    }
  }
}
