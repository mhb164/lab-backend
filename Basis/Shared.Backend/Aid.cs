
namespace Shared;

public static partial class Aid
{
    public static readonly AppInfo AppInfo = GetInfo(Assembly.GetEntryAssembly())!;

    public static AppInfo? GetInfo(Assembly? assembly)
    {
        if (assembly is null)
            return null;

        return new AppInfo(version: assembly.GetName()?.Version,
            fileVersion: assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version,
            informationalVersion: assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
            productName: assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product);
    }
}
