using System;
using AspNetBase.Core.Composition;
using AspNetBase.Presentation.Server.Extensions;
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
using Microsoft.Extensions.Logging;

namespace AspNetBase.Presentation.Server
{
  public class Startup
  {
    public ILoggerFactory LoggerFactory { get; }
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(LogLevel.Trace);
      LoggerFactory = loggerFactory;
      Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services
        .AddHttpHelpers()
        .AddEntityFramework()
        .AddIdentityAuthWithEntityFramework()
        .AddMvcWithRazorPages();

      return CompositionRoot.Initialize(services, LoggerFactory.CreateLogger<CompositionRoot>());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app
          .UseDeveloperExceptionPage()
          .UseDatabaseErrorPage();
      }
      else
      {
        app
          .UseExceptionHandler("/Home/Error")
          .UseHsts();
      }

      app
        .UseHttpsRedirection()
        .UseAuthentication()
        .UseStaticFiles()
        .UseMvc(routes =>
        {
          routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
        });
    }
  }
}
