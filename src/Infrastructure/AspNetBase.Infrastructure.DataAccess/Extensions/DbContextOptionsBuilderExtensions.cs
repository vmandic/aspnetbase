using System;
using AspNetBase.Core.Settings;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace AspNetBase.Infrastructure.DataAccess.Extensions
{
    public static class DbContextOptionsBuilderExtensions
  {
    const string DB_MIGRATIONS_ASSEMBLY_NAME = "AspNetBase.Infrastructure.DbMigrations";

    public static DbContextOptionsBuilder UseOsDependentDbProvider(
      this DbContextOptionsBuilder builder,
      DatabaseSettings dbSettings)
    {
      if (builder == null)
        throw new ArgumentNullException(nameof(builder));

      if (dbSettings == null)
        throw new ArgumentNullException(nameof(dbSettings));

      var conStr = dbSettings.GetConnectionString();

      return dbSettings.ShouldUseSqlServer() ?
        builder.UseSqlServer(conStr, opts => opts.MigrationsAssembly(DB_MIGRATIONS_ASSEMBLY_NAME)) :
        builder.UseSqlite(conStr, opts => opts.MigrationsAssembly(DB_MIGRATIONS_ASSEMBLY_NAME));
    }

    public static DbContextOptionsBuilder<AppDbContext> UseOsDependentDbProvider(
      this DbContextOptionsBuilder<AppDbContext> builder,
      DatabaseSettings dbSettings)
    {
      if (builder == null)
        throw new ArgumentNullException(nameof(builder));

      if (dbSettings == null)
        throw new ArgumentNullException(nameof(dbSettings));

      var conStr = dbSettings.GetConnectionString();

      return dbSettings.ShouldUseSqlServer() ?
        builder.UseSqlServer(conStr, opts => opts.MigrationsAssembly(DB_MIGRATIONS_ASSEMBLY_NAME)) :
        builder.UseSqlite(conStr, opts => opts.MigrationsAssembly(DB_MIGRATIONS_ASSEMBLY_NAME));
    }
  }
}
