using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Core.Composition;
using AspNetBase.Core.Settings;
using AspNetBase.Core.Settings.Extensions;
using AspNetBase.Presentation.Common.Extensions;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetBase.Presentation.Spa
{
  public class Startup
  {
    public ILoggerFactory LoggerFactory { get; }
    public IConfiguration Configuration { get; }
    public IHostingEnvironment HostEnv { get; }
    public ILogger Logger { get; set; }

    public Startup(IConfiguration config, ILoggerFactory loggerFact, IHostingEnvironment env)
    {
      LoggerFactory = loggerFact;
      Configuration = config;
      HostEnv = env;
      Logger = loggerFact.CreateLogger<Startup>();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services
        .AddOptions()
        .AddSettings(Configuration, LoggerFactory.CreateLogger<SettingsRegistration>())
        .AddHttpHelpers()
        .AddEntityFramework(SettingsLocator.Get<DatabaseSettings>(), LoggerFactory, HostEnv)
        .AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      services.AddElmahErrorLogger();

      return CompositionRoot.Initialize(
        services,
        SettingsLocator.Get<CompositionSettings>(),
        LoggerFactory.CreateLogger<CompositionRoot>());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      var arg_webpack_mode = Configuration.GetValue<string>("cli:webpack_mode") ?? "development";
      Logger.LogInformation("Setting for 'arg_webpack_mode' is set to: '{mode}'", arg_webpack_mode);

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage()
          .UseDatabaseErrorPage()
          .UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
          {
            ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "Client"),
              HotModuleReplacement = true,
              EnvParam = new { webpack_mode = arg_webpack_mode }
          });
      }
      else
      {
        app.UseHsts();
      }

      app.UseHttpsRedirection()
        .UseStaticFiles()
        .UseElmah()
        .UseMvc();
    }
  }
}
