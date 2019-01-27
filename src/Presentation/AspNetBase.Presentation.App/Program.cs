using System;
using AspNetBase.Presentation.App.Extensions;
using AspNetBase.Presentation.App.Utils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace AspNetBase.Presentation.App
{
    public class Program
  {
    public static void Main(string[] args)
    {
      Log.Logger = SerilogHelper.CreateConsoleLogger();

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

    public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
      .UseStartup<Startup>()
      .UseSerilogConfigured()
      .Build();
  }
}
