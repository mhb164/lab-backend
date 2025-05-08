
namespace Laboratory.Backend.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection ProvideServices(
         this IServiceCollection services, ILogger? logger, ConfigurationManager? configuration)
    {
        services.PrepareDefaults(logger);
        services.AddTypicalAuth(logger,configuration);
        services.AddResources(logger, configuration);
        services.AddFileShare(logger, configuration);

        services.ConfigDbContext(logger,configuration);
        services.AddScoped<IProductTypesRepository, ProductTypesRepository>();
        services.AddScoped<IProductTypesService, ProductTypesService>();

        return services;
    }

    private static void AddResources(this IServiceCollection services, ILogger? logger, ConfigurationManager? configuration)
    {
        var resourcesConfig = ResourcesOptions.ToModel(configuration?.GetSection(ResourcesOptions.ConfigName)?.Get<ResourcesOptions>());
        services.AddSingleton(resourcesConfig);
        services.AddSingleton<IResourcesService, ResourcesService>();
        logger?.LogInformation("Resources configured {Config}", resourcesConfig);
    }

    private static void AddFileShare(this IServiceCollection services, ILogger? logger, ConfigurationManager? configuration)
    {
        var config = FileShareOptions.ToModel(configuration?.GetSection(FileShareOptions.ConfigName)?.Get<FileShareOptions>());
        services.AddSingleton(config);
        services.AddSingleton<IFileShareService, FileShareService>();
        logger?.LogInformation("FileShare configured {Config}", config);
    }


    public static async Task<IServiceProvider> WarmUp(
         this IServiceProvider services, ILogger? logger)
    {
        await services.WarmUpTypicalAuth(logger);

        using (var scope = services.CreateScope())
        {
            var businessDbContext = scope.ServiceProvider.GetRequiredService<BusinessDbContext>();
            await businessDbContext.InitialAsync();

            scope.ServiceProvider.GetService<IProductTypesRepository>();
            scope.ServiceProvider.GetService<IProductTypesService>();
        }

        return services;
    }
}