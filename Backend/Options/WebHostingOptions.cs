using Backend.Config;

namespace Backend.Options;

public class WebHostingOptions
{
    public const string ConfigName = "WebHosting";

    public int? Port { get; set; }
    public int? SslPort { get; set; }
    public string? UrlPathPrefix { get; set; }

    public static WebHostingConfig ToModel(WebHostingOptions? options)
        => new WebHostingConfig(options?.Port, options?.SslPort, options?.UrlPathPrefix);
}