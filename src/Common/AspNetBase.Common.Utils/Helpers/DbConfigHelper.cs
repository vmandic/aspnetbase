using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Common.Utils.Helpers
{
  public static class DbConfigHelper
  {
    const string DB_MIGRATIONS_ASSEMBLY_NAME = "AspNetBase.Infrastructure.DbMigrations";

    public static bool IsCurrentOsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    private static string GetOsDependentConnectionString(IConfiguration config, bool alwaysUseSqlite) =>
      config.GetConnectionString(!alwaysUseSqlite && IsCurrentOsWindows ?
        "MsSqlLocalDb" :
        "SqliteLocalDb");

    public static(string conString, bool forceSqlite, string migrationsAssembly) GetDbProviderDetails(IConfiguration config)
    {
      var alwaysUseSqlite = config.GetValue<bool>("Database:AlwaysUseSqlite");

      return (
        GetOsDependentConnectionString(config, alwaysUseSqlite),
        alwaysUseSqlite,
        DB_MIGRATIONS_ASSEMBLY_NAME
      );
    }
  }
}
