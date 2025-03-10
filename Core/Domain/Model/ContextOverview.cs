namespace Backend;

public class ContextOverview
{
    public ContextOverview(string? productName, string? version, string? clientAddress, bool authenticated)
    {
        Now = DateTime.Now;
        UtcNow = DateTime.UtcNow;
        ProductName = productName ?? string.Empty;
        Version = version ?? string.Empty;
        ClientAddress = clientAddress ?? string.Empty;
        Authenticated = authenticated;
    }

    public DateTime Now { get; }
    public DateTime UtcNow { get; }
    public string ProductName { get; }
    public string Version { get; }
    public string ClientAddress { get; }
    public bool Authenticated { get; }

}
