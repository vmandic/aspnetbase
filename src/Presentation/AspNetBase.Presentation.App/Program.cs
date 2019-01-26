using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Enrichers.AspnetcoreHttpcontext;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.RollingFileAlternate;

namespace AspNetBase.Presentation.App
{
  public class Program
  {
    const string SERILOG_FILE_OUTPUT = "{Timestamp:yy-MM-dd HH:mm:ss.fff} [{Level:u3}] [SRC: {SourceContext}]{NewLine}{Message:l}{NewLine}{Exception}{Properties:j}{NewLine}{NewLine}";
    const string SERILOG_CONSOLE_OUTPUT = "{Timestamp:HH:mm:ss.fff} [{Level:u3}] [SRC: {SourceContext}]{NewLine}{Message:l}{NewLine}{Exception}{NewLine}";

    public static void Main(string[] args)
    {
      Log.Logger = ConfigConsoleLogger().CreateLogger();

      try
      {
        Log.Information("Starting web host...");
        BuildWebHost(args).Run();
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Host terminated unexpectedly!");
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }

    private static LoggerConfiguration ConfigConsoleLogger(LoggerConfiguration config = null)
    {
      return (config ?? new LoggerConfiguration())
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: SERILOG_CONSOLE_OUTPUT);
    }

    public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
      .UseStartup<Startup>()
      .UseSerilog((provider, _, baseLoggerConfig) =>
      {
        ConfigConsoleLogger(baseLoggerConfig)
          .WriteTo.Logger(fileLoggerConfig =>
            fileLoggerConfig
            .Enrich.WithExceptionDetails()
            .Enrich.WithAspnetcoreHttpcontext(provider)
            .WriteTo.RollingFileAlternate(
              "logs",
              LogEventLevel.Debug,
              SERILOG_FILE_OUTPUT,
              fileSizeLimitBytes : 1024 * 1024 * 2)); // 2MB
      })
      .Build();
  }
}
