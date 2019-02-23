using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Core.Settings;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using AspNetBase.Infrastructure.DataAccess.Enums;
using AspNetBase.Infrastructure.DataAccess.Extensions;
using ElmahCore;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Presentation.Common.Extensions
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddEntityFramework(
      this IServiceCollection services,
      DatabaseSettings dbSettings,
      ILoggerFactory loggerFactory,
      IHostingEnvironment env)
    {
      if (services == null)
        throw new ArgumentNullException(nameof(services));

      if (dbSettings == null)
        throw new ArgumentNullException(nameof(dbSettings));

      if (loggerFactory == null)
        throw new ArgumentNullException(nameof(loggerFactory));

      if (env == null)
        throw new ArgumentNullException(nameof(env));

      services.AddDbContext<AppDbContext>(opts =>
      {
        opts
          .UseLoggerFactory(loggerFactory)
          .UseOsDependentDbProvider(dbSettings);

        if (env.IsDevelopment())
          opts.EnableSensitiveDataLogging();
      });

      return services;
    }

    public static IServiceCollection AddHttpHelpers(this IServiceCollection services)
    {
      if (services == null)
        throw new ArgumentNullException(nameof(services));

      // NOTE: already injected with AddIdentity (but later)
      services.AddHttpContextAccessor();
      services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
      services.TryAddScoped<IUrlHelper>(
        s => new UrlHelper(s.GetService<IActionContextAccessor>().ActionContext));

      return services;
    }

    public static IServiceCollection AddElmahErrorLogger(this IServiceCollection services)
    {
      if (services == null)
        throw new ArgumentNullException(nameof(services));

      return services.AddElmah<XmlFileErrorLog>(opts =>
      {
        opts.CheckPermissionAction = ctx =>
          ctx.User.Identity.IsAuthenticated &&
          ctx.User.IsInRole(Roles.SystemAdministrator);

        opts.LogPath = "~/logs/errors";
      });
    }
  }
}
