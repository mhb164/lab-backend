namespace Laboratory.Backend.Options;

public class FileShareOptions
{
    public const string ConfigName = "FileShare";

    public string? Directory { get; set; }

    public static FileShareConfig ToModel(FileShareOptions? options)
    {
        return new FileShareConfig(options?.Directory);
    }
}
