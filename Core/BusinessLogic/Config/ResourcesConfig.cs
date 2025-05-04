namespace Laboratory.Backend.Config;

public class ResourcesConfig
{
    public readonly string? Directory;
    public readonly bool IsValid;

    public ResourcesConfig(string? directory)
    {
        Directory = directory?.Trim();
        IsValid = System.IO.Directory.Exists(directory);
    }

    public override string ToString()
        => $"Resources[Directory:{Directory}][IsValid:{IsValid}]";

}
