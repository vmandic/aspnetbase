using System;
using System.Collections.Generic;
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
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage()
          .UseDatabaseErrorPage();
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
