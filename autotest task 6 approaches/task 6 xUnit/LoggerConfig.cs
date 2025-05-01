using Serilog;
using System;
using System.IO;
using Xunit;

namespace task_6_xUnit
{
    public static class LoggerConfig
    {
        public static void ConfigureLogger()
        {
            var runId = Guid.NewGuid();
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var logPath = Path.Combine("Logs", $"log_{timestamp}_{runId}.txt");

            Directory.CreateDirectory("Logs");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("RunId", runId)
                .WriteTo.Console()
                .WriteTo.File(
                    path: logPath,
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 10_000_000,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 7
                )
                .CreateLogger();

            Log.Information("Logging initialized for xUnit tests (RunId: {RunId})", runId);
        }
    }
}
