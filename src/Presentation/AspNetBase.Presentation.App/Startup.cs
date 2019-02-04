using System;
using AspNetBase.Core.Composition;
using AspNetBase.Infrastructure.DataAccess.Enums;
using AspNetBase.Presentation.App.Extensions;
using AspNetBase.Presentation.App.Settings;
using ElmahCore;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        .AddSettings(Configuration)
        .AddHttpHelpers()
        .AddEntityFramework(Configuration, LoggerFactory, HostEnv)
        .AddIdentityUserRoleAuth()
        .AddMvcRazorPages()
        .AddElmahErrorLogger();

      return CompositionRoot.Initialize(services, LoggerFactory.CreateLogger<CompositionRoot>());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IOptionsMonitor<AppSettings> opts)
    {
      var appSettings = new AppSettings();
      Configuration.Bind("app", appSettings);

      var opts2 = opts.CurrentValue;

      app
        .MigrateDb()
        .SeedDb();

      if (HostEnv.IsDevelopment())
      {
        app
          .UseDeveloperExceptionPage()
          .UseDatabaseErrorPage();
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
        .UseElmah()
        .UseMvc(routes =>
        {
          routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
        });
    }
  }
}
