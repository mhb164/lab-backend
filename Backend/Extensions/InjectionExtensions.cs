namespace Laboratory.Backend.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection ProvideServices(
         this IServiceCollection services, ILogger? logger, ConfigurationManager? configuration)
    {
        services.PrepareDefaults(logger);
        services.AddTypicalAuth(logger,configuration);

        services.ConfigDbContext(logger,configuration);
        services.AddScoped<IProductTypesRepository, ProductTypesRepository>();
        services.AddScoped<IProductTypesService, ProductTypesService>();

        return services;
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