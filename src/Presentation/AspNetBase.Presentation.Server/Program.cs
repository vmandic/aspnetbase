using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.RollingFileAlternate;

namespace AspNetBase.Presentation.Server
{
  public class Program
  {
    static readonly string SERILOG_OUTPUT = "{Timestamp:yy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message}{NewLine}{Exception}";

    public static void Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: SERILOG_OUTPUT)
        .WriteTo.RollingFileAlternate("logs", LogEventLevel.Debug, SERILOG_OUTPUT, fileSizeLimitBytes: 1024 * 1024 * 2) // 2MB
        .CreateLogger();

      try
      {
        Log.Information("Starting web host");
        BuildWebHost(args).Run();
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Host terminated unexpectedly");
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }

    public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
      .UseStartup<Startup>()
      .UseSerilog()
      .Build();
  }
}
