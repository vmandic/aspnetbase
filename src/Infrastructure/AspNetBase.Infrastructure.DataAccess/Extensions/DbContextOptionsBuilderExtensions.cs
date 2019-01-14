using System;
using System.Runtime.InteropServices;
using AspNetBase.Common.Utils.Helpers;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Infrastructure.DataAccess.Extensions
{
  public static class DbContextOptionsBuilderExtensions
  {
    public static DbContextOptionsBuilder UseOsDependentDbProvider(
      this DbContextOptionsBuilder builder,
      IConfiguration config)
    {
      if (builder == null)
        throw new ArgumentNullException(nameof(builder));

      if (config == null)
        throw new ArgumentNullException(nameof(config));

      var (connectionString, forceSqlite, migrationsAssembly) = DbConfigHelper.GetDbProviderDetails(config);

      return !forceSqlite && DbConfigHelper.IsCurrentOsWindows ?
        builder.UseSqlServer(connectionString, opts => opts.MigrationsAssembly(migrationsAssembly)) :
        builder.UseSqlite(connectionString, opts => opts.MigrationsAssembly(migrationsAssembly));
    }

    public static DbContextOptionsBuilder<AppDbContext> UseOsDependentDbProvider(
      this DbContextOptionsBuilder<AppDbContext> builder,
      IConfiguration config)
    {
      if (builder == null)
        throw new ArgumentNullException(nameof(builder));

      if (config == null)
        throw new ArgumentNullException(nameof(config));

      var (connectionString, forceSqlite, migrationsAssembly) = DbConfigHelper.GetDbProviderDetails(config);

      return !forceSqlite && DbConfigHelper.IsCurrentOsWindows ?
        builder.UseSqlServer(connectionString, opts => opts.MigrationsAssembly(migrationsAssembly)) :
        builder.UseSqlite(connectionString, opts => opts.MigrationsAssembly(migrationsAssembly));
    }
  }
}
