using System;
using System.IO;
using System.Reflection;
using AspNetBase.Common.Utils.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HostFiltering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Enrichers.AspnetcoreHttpcontext;

namespace AspNetBase.Presentation.Common.Extensions
{
  public static class IWebHostBuilderExtensions
  {
    public static IWebHostBuilder UseSerilogConfigured(this IWebHostBuilder webHostBuilder) =>
      webHostBuilder.UseSerilog((serviceProvider, _, baseConfig) =>
      {
        SerilogHelper.ConfigConsoleLogger(baseConfig)
          // NOTE: added an additional file logger with more exception and HTTP context details
          .WriteTo.Logger(config => SerilogHelper.ConfigRichFileLogger(config, serviceProvider));
      });

    public static IWebHostBuilder ReconfigureConfiguration(this IWebHostBuilder webHostBuilder, string[] args) =>
      webHostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
      {
        config.Sources.Clear();

        var env = hostingContext.HostingEnvironment;

        var baseAppSettingsPath = Path.Combine(
          hostingContext.HostingEnvironment.ContentRootPath,
          "../../Core/AspNetBase.Core.Settings");

        config
          .AddJsonFile($"{baseAppSettingsPath}/appsettings.base.json", optional : true, reloadOnChange : true)
          .AddJsonFile($"{baseAppSettingsPath}/appsettings.base.{env.EnvironmentName}.json", optional : true, reloadOnChange : true)
          .AddJsonFile($"appsettings.json", optional : true, reloadOnChange : true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional : true, reloadOnChange : true);

        if (env.IsDevelopment())
        {
          var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
          if (appAssembly != null)
          {
            config.AddUserSecrets(appAssembly, optional : true);
          }
        }

        config.AddEnvironmentVariables();

        if (args != null)
          config.AddCommandLine(args);
      });

  }
}
