using Microsoft.Extensions.Configuration;
using Backend.Auth;
using Backend.Interfaces;
using Backend.Options;
using Backend.Repositories;
using Backend.Services;

namespace Backend.Extensions;

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
        });


        ConfigureDbContext(services, configuration);
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IArchivedUserTokenRepository, ArchivedUserTokenRepository>();

        services.AddScoped<IProductTypesRepository, ProductTypesRepository>();
        services.AddScoped<IProductTypesService, ProductTypesService>();

        return services;
    }

    private static void ConfigureDbContext(IServiceCollection services, ConfigurationManager? configuration)
    {
        var repositoryConfig = BackendDbOptions.ToModel(configuration?.GetSection(BackendDbOptions.ConfigName)?.Get<BackendDbOptions>());

        if (repositoryConfig.Provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            services.AddScoped<BackendDbContext>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<BackendSqlDbContext>>();
                return new BackendSqlDbContext(logger, repositoryConfig.ConnectionString);
            });
    }

    public static async Task<IServiceProvider> WarmUp(
         this IServiceProvider services, ConfigurationManager? configuration)
    {
        using (var scope = services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BackendDbContext>();
            await dbContext.InitialAsync();

            var adminDefaultPassword = configuration?.GetSection($"AdminDefaultPassword")?.Value ?? string.Empty;
            var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
            await authService.EnsureAdminUserExistsAsync(adminDefaultPassword);

            scope.ServiceProvider.GetService<IProductTypesRepository>();
            scope.ServiceProvider.GetService<IProductTypesService>();
        }

        return services;
    }
}