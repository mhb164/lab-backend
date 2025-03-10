using System.Reflection;

namespace Backend.Extensions;

public static partial class Aid
{
    public static string? AppVersion
        => Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString();

    public static string? AppFileVersion
        => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

    public static string? AppInformationalVersion
        => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

    public static string? ProductName
        => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
}
