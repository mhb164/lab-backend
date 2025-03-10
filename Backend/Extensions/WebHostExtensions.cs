using Microsoft.AspNetCore.Server.HttpSys;
using Backend.Config;

namespace Backend.Extensions;

public static class WebHostExtensions
{
    public static void ConfigWebHost(this IWebHostBuilder hostBuilder, WebHostingConfig hostingConfig)
    {
#pragma warning disable CA1416
        hostBuilder.UseHttpSys(options =>
        {
            options.Authentication.Schemes = AuthenticationSchemes.None;
            options.Authentication.AllowAnonymous = true;
            options.MaxConnections = null;
            options.MaxRequestBodySize = 30000000;
            options.UrlPrefixes.Add($"http://*:{hostingConfig.Port}/{hostingConfig.UrlPathPrefix}");
            options.UrlPrefixes.Add($"https://*:{hostingConfig.SslPort}/{hostingConfig.UrlPathPrefix}");
        });
#pragma warning restore CA1416
    }

}