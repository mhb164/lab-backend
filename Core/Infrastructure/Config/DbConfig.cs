namespace Laboratory.Backend.Config;

public class DbConfig
{
    private static readonly HashSet<string> ValidProviders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "SqlServer",
        "Sqlite",
    };

    public readonly string Provider;
    public readonly string ConnectionString;

    public DbConfig(string? provider, string? connectionString)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(provider);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString);

        provider = provider.Trim();
        if (!ValidProviders.Contains(provider))
            throw new InvalidOperationException($"Db.{nameof(Provider)} is not valid ({provider})!");

        Provider = provider;
        ConnectionString = connectionString;
    }

    public override string ToString()
        => $"Repository[Provider:{Provider}][ConnectionString:{ConnectionString}]";

    public bool IsSqlServer => Provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase);
    public bool IsSqlite => Provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase);
}
