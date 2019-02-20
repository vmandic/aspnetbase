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
using AspNetBase.Presentation.App.Constants;
using AspNetBase.Presentation.App.Resources;
using AspNetBase.Presentation.App.Utils;
using ElmahCore;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Presentation.App.Extensions
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

      services.AddSingleton<IDesignTimeDbContextFactory<AppDbContext>, DesignTimeDbContextFactory>();

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

    public static IServiceCollection AddIdentityUserRoleAuth(this IServiceCollection services)
    {
      if (services == null)
        throw new ArgumentNullException(nameof(services));

      services
        .AddIdentity<AppUser, AppRole>(opts =>
        {
          // NOTE: adds authentication, cookies, and identity services
          opts.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>() // NOTE: adds default User and Role store implementations
        .AddDefaultTokenProviders(); // NOTE: adds the identity default token generators

      // NOTE: configure additional authorization rules and policies
      services.AddAuthorization(opts =>
      {
        opts.AddPolicy(
          AppAuthorizationPolicies.RequiresSystemAdministrator,
          p => p.RequireRole(Roles.SystemAdministrator.ToString()));
      });

      // NOTE: overrides the AddIdentity defaults for the added cookies with AddIdentity
      services.ConfigureApplicationCookie(opts =>
      {
        opts.LoginPath = $"/Identity/Account/Login";
        opts.LogoutPath = $"/Identity/Account/Logout";
        opts.AccessDeniedPath = $"/Identity/Account/AccessDenied";
      });

      // ? TODO: add JwtBearer Auth and auth policies

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

    public static IServiceCollection AddMvcRazorPagesWithLocalization(
      this IServiceCollection services,
      LocalizationSettings localizationSettings)
    {
      if (services == null)
        throw new ArgumentNullException(nameof(services));

      if (localizationSettings == null)
        throw new ArgumentNullException(nameof(localizationSettings));

      services
        .AddMvc()
        .AddMvcLocalization(opts =>
          opts.DataAnnotationLocalizerProvider =
          (type, factory) => factory.Create(typeof(SharedResources)))
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
        .AddRazorPagesOptions(options =>
        {
          options.AllowAreas = true;

          options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
          options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
          options.Conventions.AuthorizeAreaFolder("Admin", "/", AppAuthorizationPolicies.RequiresSystemAdministrator);
        });

      services.Configure<RequestLocalizationOptions>(opts => {
        LocalizationHelper.ConfigureLocalizationOptions(
            opts,
            localizationSettings);
      });

      return services.AddLocalization();
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
