namespace Laboratory.Backend.Config;

public class WebHostingConfig
{
    public const string DefaultUrlPathPrefix = "labapi";

    public readonly int Port;
    public readonly int SslPort;
    public readonly string UrlPathPrefix;

    public WebHostingConfig(int? port, int? sslPort, string? urlPathPrefix)
    {
        Port = port ?? 80;
        SslPort = sslPort ?? 443;
        UrlPathPrefix = urlPathPrefix ?? DefaultUrlPathPrefix;
    }

    public override string ToString()
        => $"WebHosting:[Port:{Port}][SslPort:{SslPort}][UrlPathPrefix:{UrlPathPrefix}]";
}