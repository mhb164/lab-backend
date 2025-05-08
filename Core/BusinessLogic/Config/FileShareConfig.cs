namespace Laboratory.Backend.Config;

public class FileShareConfig
{
    public readonly string Directory;
    public readonly bool IsValid;

    public FileShareConfig(string? directory)
    {
        Directory = directory?.Trim() ?? string.Empty;
        IsValid = System.IO.Directory.Exists(directory);
    }

    public override string ToString()
        => $"FileShare[Directory:{Directory}][IsValid:{IsValid}]";

}
