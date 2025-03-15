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

    public static void ConfigureAppStart()
    {
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

        ThreadPool.SetMaxThreads(32767, 16184);
        ThreadPool.SetMinThreads(2048, 2048);

        ServicePointManager.UseNagleAlgorithm = true;
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.DefaultConnectionLimit = 8092;
    }
}
