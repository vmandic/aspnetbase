using AspNetBase.Presentation.App.Utils;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Enrichers.AspnetcoreHttpcontext;

namespace AspNetBase.Presentation.App.Extensions
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

  }
}
