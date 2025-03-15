namespace Shared.Model;

public sealed class AppInfo
{
    public readonly Version? Version;
    public readonly string? FileVersion;
    public readonly string? InformationalVersion;
    public readonly string? ProductName;

    public readonly string Text;

    public AppInfo(Version? version, string? fileVersion, string? informationalVersion, string? productName)
    {
        Version = version;
        FileVersion = fileVersion;
        InformationalVersion = informationalVersion;
        ProductName = productName;

        Text = $"{ProductName} v{InformationalVersion}";
    }

    public override string ToString() 
        => Text;
}
