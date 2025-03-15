namespace Laboratory.Backend.Options;

public class WebHostingOptions
{
    public const string ConfigName = "WebHosting";
    public const string DefaultUrlPathPrefix = "labapi";

    public bool? Enabled { get; set; }
    public int? Port { get; set; }
    public int? SslPort { get; set; }
    public string? UrlPathPrefix { get; set; }

    public static WebHostingConfig ToModel(WebHostingOptions? options)
        => new WebHostingConfig(options?.Enabled, options?.Port, options?.SslPort, options?.UrlPathPrefix?? DefaultUrlPathPrefix);
}