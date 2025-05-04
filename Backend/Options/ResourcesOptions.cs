namespace Laboratory.Backend.Options;

public sealed class ResourcesOptions
{
    public const string ConfigName = "Resources";

    public string? Directory { get; set; }

    public static ResourcesConfig ToModel(ResourcesOptions? options)
    {
        return new ResourcesConfig(options?.Directory);
    }
}
