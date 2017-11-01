using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace CustomSerilogFormatter
{
    public static class CustomSerilogConfigurator
    {
        public static void Setup(string appName, string appVersion, bool logToFile, string logFilePath = ".\\logs\\", bool renderMessageTemplate = false)
        {
            var formatter = new CustomSerilogFormatter(appName, appVersion, renderMessageTemplate);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("IdentityServer4", LogEventLevel.Warning)
                .MinimumLevel.Override("Healthcheck.HealtchCheckMiddleware", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .WriteTo.Logger(lc =>
                {
                    lc.Filter.ByExcluding(Matching.FromSource("performance"));
                    lc.WriteTo.Console(formatter);
                    if (logToFile)
                    {
                        lc.WriteTo.RollingFile(formatter, Path.Combine(logFilePath, "log-{Date}.log"));
                    }
                })
                .WriteTo.Logger(lc =>
                {
                    lc.Filter.ByIncludingOnly(Matching.FromSource("performance"));
                    lc.WriteTo.Console(formatter);
                    if (logToFile)
                    {
                        lc.WriteTo.RollingFile(formatter, Path.Combine(logFilePath, "log-perf-{Date}.log"));
                    }
                })
                .CreateLogger();
        }
    }
}