using System.IO;
using Serilog;
using Serilog.Filters;

namespace CustomSerilogFormatter
{
    public static class CustomSerilogConfigurator
    {
        public static void Setup(string appName, bool logToFile, string logFilePath = ".\\logs\\", bool renderMessageTemplate = false)
        {
            var formatter = new CustomSerilogFormatter(appName, renderMessageTemplate);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
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