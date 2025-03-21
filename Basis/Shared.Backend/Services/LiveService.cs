namespace Shared.Services;

public class LiveService : BackgroundService
{
    private readonly ILogger? _logger;
    private readonly AppInfo _appInfo;
    public LiveService(ILogger<LiveService>? logger, AppInfo appInfo)
    {
        _logger = logger;
        _appInfo = appInfo ?? throw new ArgumentNullException(nameof(appInfo));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger?.LogInformation("[LIVE] {AppInfo} is starting.", _appInfo);

        stoppingToken.Register(() => _logger?.LogInformation("[LIVE] {AppInfo} is stopping.", _appInfo));

        while (!stoppingToken.IsCancellationRequested)
        {
            //_logger?.LogInformation("[LIVE] {AppInfo} is doing background work.", _appInfo);

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

        _logger?.LogInformation("[LIVE] {AppInfo} has stopped.", _appInfo);
    }
}
