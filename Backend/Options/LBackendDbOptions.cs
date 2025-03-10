using Backend.Config;

namespace Backend.Options;

public class BackendDbOptions
{
    public const string ConfigName = "Repository";

    public string? Provider { get; set; }
    public string? ConnectionString { get; set; }

    public static BackendDbConfig ToModel(BackendDbOptions? options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return new BackendDbConfig(options.Provider, options.ConnectionString);
    }
}
