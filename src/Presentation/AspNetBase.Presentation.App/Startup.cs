using System;
using AspNetBase.Core.Composition;
using AspNetBase.Core.Settings;
using AspNetBase.Core.Settings.Extensions;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using AspNetBase.Presentation.App.Extensions;
using AspNetBase.Presentation.App.Utils;
using AspNetBase.Presentation.Common.Extensions;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetBase.Presentation.App
{
  public class Startup
  {
    public ILoggerFactory LoggerFactory { get; }
    public IConfiguration Configuration { get; }
    public IHostingEnvironment HostEnv { get; }

    public Startup(IConfiguration config, ILoggerFactory loggerFact, IHostingEnvironment env)
    {
      LoggerFactory = loggerFact;
      Configuration = config;
      HostEnv = env;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services
        .AddOptions()
        .AddSettings(Configuration, LoggerFactory.CreateLogger<SettingsRegistration>())
        .AddHttpHelpers()
        .AddEntityFramework(SettingsLocator.Get<DatabaseSettings>(), LoggerFactory, HostEnv)
        .AddIdentityUserRoleAuth()
        .AddMvcRazorPagesWithLocalization(SettingsLocator.Get<LocalizationSettings>())
        .AddElmahErrorLogger();

      // NOTE: required for database migrations
      services.AddSingleton<IDesignTimeDbContextFactory<AppDbContext>, DesignTimeDbContextFactory>();

      return CompositionRoot.Initialize(
        services,
        SettingsLocator.Get<CompositionSettings>(),
        LoggerFactory.CreateLogger<CompositionRoot>());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(
      IApplicationBuilder app,
      AppSettings appSettings,
      IOptions<RequestLocalizationOptions> localizationOpts)
    {
      if (appSettings == null)
        throw new ArgumentNullException(nameof(appSettings));

      if (localizationOpts == null || localizationOpts.Value == null)
        throw new ArgumentNullException(nameof(localizationOpts));

      app
        .MigrateDb(appSettings.Database)
        .SeedDb(appSettings.Database);

      if (HostEnv.IsDevelopment())
      {
        app
          .UseDeveloperExceptionPage()
          .UseDatabaseErrorPage();

        // allows debugging .ts files over sourcemaps
        app.UseStaticFiles("/Client/ts");
      }
      else
      {
        app
          .UseHsts()
          .UseExceptionHandler("/Home/Error");
      }

      app
        .UseHttpsRedirection()
        .UseAuthentication()
        .UseStaticFiles()
        .UseRequestLocalization(localizationOpts.Value)
        .UseElmah()
        .UseMvcWithDefaultRoute();
    }
  }
}
