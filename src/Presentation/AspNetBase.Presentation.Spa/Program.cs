using System;
using AspNetBase.Common.Utils.Helpers;
using AspNetBase.Presentation.Common.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace AspNetBase.Presentation.Spa
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Log.Logger = SerilogHelper
        .CreateConsoleLogger()
        .ForContext<Program>();

      try
      {
        Log.Information("Starting Spa web host...");
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

    public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
      .UseAppSettingsConfiguration(args)
      .UseStartup<Startup>()
      .UseSerilogConfigured()
      .Build();
  }
}
