using System.Runtime.InteropServices;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Core.Settings;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Infrastructure.Utils
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
      var dbSettings = config.Bind<DatabaseSettings>("app:database");

      return (
        GetOsDependentConnectionString(config, dbSettings.AlwaysUseSqlite),
        dbSettings.AlwaysUseSqlite,
        DB_MIGRATIONS_ASSEMBLY_NAME
      );
    }
  }
}
