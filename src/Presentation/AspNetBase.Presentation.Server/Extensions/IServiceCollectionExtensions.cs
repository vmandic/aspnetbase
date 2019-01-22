using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using AspNetBase.Infrastructure.DataAccess.Extensions;
using AspNetBase.Presentation.Server.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Presentation.Server.Extensions
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddEntityFramework(
      this IServiceCollection services,
      IConfiguration config,
      ILoggerFactory loggerFactory,
      IHostingEnvironment env)
    {
      services.AddSingleton<IDesignTimeDbContextFactory<AppDbContext>, DesignTimeDbContextFactory>();

      services.AddDbContext<AppDbContext>(opts =>
      {
        opts
          .UseLoggerFactory(loggerFactory)
          .UseOsDependentDbProvider(config);

        if (env.IsDevelopment())
          opts.EnableSensitiveDataLogging();
      });

      return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
      services
        .AddIdentity<AppUser, AppRole>(opts =>
        { // NOTE: adds authentication, cookies, and identity services
          opts.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>() // NOTE: adds default User and Role store implementations
        .AddDefaultTokenProviders(); // NOTE: adds the identity default token generators

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
      // NOTE: already injected with AddIdentity (but later)
      services.AddHttpContextAccessor();
      services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
      services.TryAddScoped<IUrlHelper>(
        s => new UrlHelper(s.GetService<IActionContextAccessor>().ActionContext));

      return services;
    }

    public static IServiceCollection AddMvcWithRazorPages(this IServiceCollection services)
    {
      services
        .AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
        .AddRazorPagesOptions(options =>
        {
          options.AllowAreas = true;
          options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
          options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");

          options.Conventions.AuthorizeAreaFolder("Admin", "/");
        });

      return services;
    }
  }
}
