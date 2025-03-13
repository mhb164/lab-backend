namespace Laboratory.Backend.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection ProvideServices(
         this IServiceCollection services, ConfigurationManager? configuration)
    {
        services.AddSingleton(JwtOptions.ToModel(configuration?.GetSection(JwtOptions.ConfigName)?.Get<JwtOptions>()));
        services.AddCors();

        // Set the JSON serializer options
        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = false;
            options.SerializerOptions.PropertyNamingPolicy = null;
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.IncludeFields = false;
        });


        services.ConfigDbContext(configuration);
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserContext, UserContext>();       

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IArchivedUserTokenRepository, ArchivedUserTokenRepository>();

        services.AddScoped<IProductTypesRepository, ProductTypesRepository>();
        services.AddScoped<IProductTypesService, ProductTypesService>();

        return services;
    }   

    public static async Task<IServiceProvider> WarmUp(
         this IServiceProvider services, ConfigurationManager? configuration)
    {
        using (var scope = services.CreateScope())
        {
            var authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            await authDbContext.InitialAsync();

            var businessDbContext = scope.ServiceProvider.GetRequiredService<BusinessDbContext>();
            await businessDbContext.InitialAsync();

            var adminDefaultPassword = configuration?.GetSection($"AdminDefaultPassword")?.Value ?? string.Empty;
            var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
            await authService.EnsureDefaultUsersExistsAsync(adminDefaultPassword);

            scope.ServiceProvider.GetService<IProductTypesRepository>();
            scope.ServiceProvider.GetService<IProductTypesService>();
        }

        return services;
    }
}