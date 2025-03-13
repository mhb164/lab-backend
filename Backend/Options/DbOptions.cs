namespace Laboratory.Backend.Options;

public abstract class DbOptions
{
    public string? Provider { get; set; }
    public string? ConnectionString { get; set; }

    public static DbConfig ToModel(DbOptions? options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return new DbConfig(options.Provider, options.ConnectionString);
    }
}

public class BusinessDbOptions : DbOptions
{
    public const string ConfigName = "Repository";
}

public class AuthDbOptions : DbOptions
{
    public const string ConfigName = "AuthRepository";
}