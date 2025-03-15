using Serilog;
using Serilog.Events;

namespace Laboratory.Backend.Extensions;

public static class LoggerExtensions
{
    public static Microsoft.Extensions.Logging.ILogger ConfigureLogger(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        var logPrefix = Aid.AppInfo.ProductName!.Replace(" ","");
        var loggingDirectory = configuration.GetSection($"LogDirectory")?.Value ?? string.Empty;

        var resolvedLoggingDirectory = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, loggingDirectory));
        if (!Directory.Exists(resolvedLoggingDirectory))
            Directory.CreateDirectory(resolvedLoggingDirectory);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .WriteTo.File($"{resolvedLoggingDirectory}\\{logPrefix}Logs-.log",
                          rollingInterval: RollingInterval.Day,
                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}]{Scope} {Message:lj}{NewLine}{Exception}",
                          rollOnFileSizeLimit: true, fileSizeLimitBytes: 10 * 1024 * 1024, shared: true, retainedFileCountLimit: 180)
            .WriteTo.File($"{resolvedLoggingDirectory}\\{logPrefix}Errors-.log",
                          restrictedToMinimumLevel: LogEventLevel.Error,
                          rollingInterval: RollingInterval.Day,
                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}]{Scope} {Message:lj}{NewLine}{Exception}",
                          rollOnFileSizeLimit: true, fileSizeLimitBytes: 10 * 1024 * 1024, shared: true, retainedFileCountLimit: null)
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}]{Scope} {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        hostBuilder.UseSerilog();
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog());
        var logger = loggerFactory.CreateLogger(logPrefix);

        return logger;
    }

    public static ValueTask FinalizeLoggerAsync(this IHostBuilder hostBuilder)
        => Log.CloseAndFlushAsync();
}