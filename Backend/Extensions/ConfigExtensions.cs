using Serilog;
using Serilog.Events;

namespace Backend.Extensions;

public static class ConfigExtensions
{
    public static void ConfigureLogger(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        var loggingDirectory = configuration.GetSection($"LogDirectory")?.Value ?? string.Empty;

        var resolvedLoggingDirectory = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, loggingDirectory));
        if (!Directory.Exists(resolvedLoggingDirectory))
            Directory.CreateDirectory(resolvedLoggingDirectory);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .WriteTo.File($"{resolvedLoggingDirectory}\\BackendLogs-.log",
                          rollingInterval: RollingInterval.Day,
                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}]{Scope} {Message:lj}{NewLine}{Exception}",
                          rollOnFileSizeLimit: true, fileSizeLimitBytes: 10 * 1024 * 1024, shared: true, retainedFileCountLimit: 180)
            .WriteTo.File($"{resolvedLoggingDirectory}\\BackendErrors-.log",
                          restrictedToMinimumLevel: LogEventLevel.Error,
                          rollingInterval: RollingInterval.Day,
                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}]{Scope} {Message:lj}{NewLine}{Exception}",
                          rollOnFileSizeLimit: true, fileSizeLimitBytes: 10 * 1024 * 1024, shared: true, retainedFileCountLimit: null)
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}]{Scope} {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        hostBuilder.UseSerilog();
    }
}