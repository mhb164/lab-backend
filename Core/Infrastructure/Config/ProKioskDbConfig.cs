namespace Backend.Config;

public class BackendDbConfig
{
    private static readonly HashSet<string> ValidProviders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "SqlServer",
    };

    public readonly string Provider;
    public readonly string ConnectionString;

    public BackendDbConfig(string? provider, string? connectionString)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(provider);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString);

        provider = provider.Trim();
        if (!ValidProviders.Contains(provider))
            throw new InvalidOperationException($"BackendDb.{nameof(Provider)} is not valid ({provider})!");

        Provider = provider;
        ConnectionString = connectionString;
    }

    public override string ToString()
        => $"Repository[Provider:{Provider}][ConnectionString:{ConnectionString}]";
}
