using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.RollingFileAlternate;

namespace AspNetBase.Presentation.Server
{
  public class Program
  {
    const string SERILOG_FILE_OUTPUT = "{Timestamp:yy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message}{NewLine}{Exception} {Properties:j}";
    const string SERILOG_CONSOLE_OUTPUT = "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message}{NewLine}{Exception}";

    public static void Main(string[] args)
    {
      ConfigureSerilog();

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

    private static void ConfigureSerilog()
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: SERILOG_CONSOLE_OUTPUT)
        .WriteTo.Logger(loggerConfig =>
          loggerConfig
            .Enrich.WithExceptionDetails()
            .WriteTo.RollingFileAlternate(
              "logs",
              LogEventLevel.Debug,
              SERILOG_FILE_OUTPUT,
              fileSizeLimitBytes : 1024 * 1024 * 2) // 2MB
        )
        .CreateLogger();
    }

    public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
      .UseStartup<Startup>()
      .UseSerilog()
      .Build();
  }
}
