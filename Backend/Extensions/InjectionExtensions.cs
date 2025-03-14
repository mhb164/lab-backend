namespace Laboratory.Backend.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection ProvideServices(
         this IServiceCollection services, ConfigurationManager? configuration)
    {
        services.AddCors();
        services.AddTypicalAuth(configuration);

        // Set the JSON serializer options
        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = false;
            options.SerializerOptions.PropertyNamingPolicy = null;
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.IncludeFields = false;
        });     

        services.ConfigDbContext(configuration);
        services.AddScoped<IProductTypesRepository, ProductTypesRepository>();
        services.AddScoped<IProductTypesService, ProductTypesService>();

        return services;
    }

    public static async Task<IServiceProvider> WarmUp(
         this IServiceProvider services, ConfigurationManager? configuration)
    {
        await services.WarmUpTypicalAuth(configuration?.GetSection($"AdminDefaultPassword")?.Value ?? string.Empty);
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