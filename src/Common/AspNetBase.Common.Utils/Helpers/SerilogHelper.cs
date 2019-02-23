using System;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.AspnetcoreHttpcontext;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.RollingFileAlternate;

namespace AspNetBase.Common.Utils.Helpers
{
  public class SerilogHelper
  {
    const int SERILOG_ROLLING_FILE_SIZE_BYTES = 1024 * 1024 * 2; // 2 MB
    const string SERILOG_FILE_OUTPUT = "{Timestamp:yy-MM-dd HH:mm:ss.fff} [{Level:u3}] [SRC: {SourceContext}]{NewLine}{Message:l}{NewLine}{Exception}{Properties:j}{NewLine}{NewLine}";
    const string SERILOG_CONSOLE_OUTPUT = "{Timestamp:HH:mm:ss.fff} [{Level:u3}] [SRC: {SourceContext}]{NewLine}{Message:l}{NewLine}{Exception}{NewLine}";

    public static LoggerConfiguration ConfigConsoleLogger(LoggerConfiguration config = null) =>
      (config ?? new LoggerConfiguration())
      .MinimumLevel.Debug()
      .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
      .Enrich.FromLogContext()
      .WriteTo.Console(outputTemplate: SERILOG_CONSOLE_OUTPUT);

    public static LoggerConfiguration ConfigRichFileLogger(
        LoggerConfiguration config,
        IServiceProvider serviceProvider) =>
      (config ?? new LoggerConfiguration())
      .Enrich.WithExceptionDetails()
      .Enrich.WithAspnetcoreHttpcontext(serviceProvider ??
        throw new ArgumentNullException(nameof(serviceProvider)))
      .WriteTo.RollingFileAlternate(
        "logs",
        LogEventLevel.Debug,
        SERILOG_FILE_OUTPUT,
        fileSizeLimitBytes : SERILOG_ROLLING_FILE_SIZE_BYTES);

    public static Logger CreateConsoleLogger() =>
      ConfigConsoleLogger().CreateLogger();
  }
}
