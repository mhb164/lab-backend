namespace Shared.Extensions;

public static class WebHostExtensions
{
    public static void ConfigWebHost(this IWebHostBuilder hostBuilder, ILogger? logger, WebHostingConfig hostingConfig)
    {
        if (!hostingConfig.Enabled)
        {
            logger?.LogInformation("WebHost(HttpSys) is disable!");
            return;
        }

#pragma warning disable CA1416
        hostBuilder.UseHttpSys(options =>
        {
            options.Authentication.Schemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.None;
            options.Authentication.AllowAnonymous = true;
            options.MaxConnections = null;
            options.MaxRequestBodySize = 30000000;
            options.UrlPrefixes.Add($"http://*:{hostingConfig.Port}/{hostingConfig.UrlPathPrefix}");
            options.UrlPrefixes.Add($"https://*:{hostingConfig.SslPort}/{hostingConfig.UrlPathPrefix}");
        });
#pragma warning restore CA1416
    }

}