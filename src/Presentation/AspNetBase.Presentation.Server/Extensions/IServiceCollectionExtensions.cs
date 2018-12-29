using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Core.Composition;
using AspNetBase.Infrastructure.DataAccess.Data;
using AspNetBase.Infrastructure.DataAccess.Entities;
using AspNetBase.Presentation.Server.Extensions;
using AspNetBase.Presentation.Server.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetBase.Presentation.Server.Extensions
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddEntityFramework(this IServiceCollection services)
    {
      services.AddSingleton<IDesignTimeDbContextFactory<AppDbContext>, DesignTimeDbContextFactory>();

      services.AddScoped(
        s => s.GetService<IDesignTimeDbContextFactory<AppDbContext>>().CreateDbContext(null));

      return services;
    }

    public static IServiceCollection AddIdentityAuthWithEntityFramework(this IServiceCollection services)
    {
      services.AddIdentity<AppUser, AppRole>()    // adds authentication, cookies, and identity services
        .AddEntityFrameworkStores<AppDbContext>() // adds default User and Role store implementations
        .AddDefaultTokenProviders();              // adds the identity default token generators

      // TODO: add custom UserClaimsFactory implementation to support additional Claims during sign in

      // overrides the AddIdentity defaults for the added cookies
      services.ConfigureApplicationCookie(options =>
      {
        options.LoginPath = $"/Identity/Account/Login";
        options.LogoutPath = $"/Identity/Account/Logout";
        options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
      });

      return services;
    }

    public static IServiceCollection AddHttpHelpers(this IServiceCollection services)
    {
      services.AddHttpContextAccessor();
      services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
      services.AddScoped<IUrlHelper>(
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
        });

      return services;
    }
  }
}
