var builder = WebApplication.CreateBuilder(args);
var logger = builder.Host.ConfigureLogger(builder.Configuration);

logger.LogInformation("{AppInfo} started.", Aid.AppInfo);
try
{
    var hostingConfig = WebHostingOptions.ToModel(builder.Configuration?.GetSection(WebHostingOptions.ConfigName)?.Get<WebHostingOptions>());
    builder.WebHost.ConfigWebHost(logger, hostingConfig);

    builder.Services.ProvideServices(logger, builder.Configuration);

    var app = builder.Build();
    await app.Services.WarmUp(logger);
    app.PrepareDefaults(logger);
    app.UseMiddleware<AuthMiddleware>();
    app.MapEndpoints();
    await app.RunAsync();

    logger.LogInformation("{AppInfo} stopped.", Aid.AppInfo);
}
catch (Exception ex)
{
    logger.LogError(ex, "{AppInfo} exception.", Aid.AppInfo);
}
finally
{
    await builder.Host.FinalizeLoggerAsync();
}