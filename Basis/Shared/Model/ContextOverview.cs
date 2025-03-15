namespace Shared.Model;

public class ContextOverview
{
    private readonly DateTime _createdAtUtc;

    public ContextOverview()
        : this(string.Empty, string.Empty, string.Empty, false) { }

    public ContextOverview(string? productName, string? version, string? clientIpAddress, bool authenticated)
    {
        _createdAtUtc = DateTime.UtcNow;

        Now = DateTime.Now;
        UtcNow = DateTime.UtcNow;
        ProductName = productName ?? string.Empty;
        Version = version ?? string.Empty;
        ClientIpAddress = clientIpAddress ?? string.Empty;
        Authenticated = authenticated;
    }

    public DateTime Now { get; set; }
    public DateTime UtcNow { get; set; }
    public string ProductName { get; set; }
    public string Version { get; set; }
    public string ClientIpAddress { get; set; }
    public bool Authenticated { get; set; }

    public TimeSpan LifeTime => DateTime.UtcNow - _createdAtUtc;
}
