using TypicalAuth.Config;

namespace Laboratory.Backend.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddTypicalAuth(
        this IServiceCollection services, ConfigurationManager? configuration)
    {
        var authDbOptions = AuthDbOptions.ToModel(configuration?.GetSection(AuthDbOptions.ConfigName)?.Get<AuthDbOptions>());
        var jwtConfig = JwtOptions.ToModel(configuration?.GetSection(JwtOptions.ConfigName)?.Get<JwtOptions>());
        services.AddTypicalAuth(jwtConfig: jwtConfig, authDbContextFactory: AuthDbContextFactory(authDbOptions));
        Log.Information("Auth {Provider} configured", authDbOptions.Provider);
        return services;
    }
    
    private static Func<IServiceProvider, AuthDbContext> AuthDbContextFactory(DbConfig dbConfig)
    {
        if (dbConfig.IsSqlServer)
        {
            return (provider) =>
            {
                var logger = provider.GetRequiredService<ILogger<AuthSqlDbContext>>();
                return new AuthSqlDbContext(logger, dbConfig.ConnectionString);
            };
        }

        if (dbConfig.IsSqlite)
        {
            return (provider) =>
            {
                var logger = provider.GetRequiredService<ILogger<AuthSqliteDbContext>>();
                return new AuthSqliteDbContext(logger, dbConfig.ConnectionString);
            };
        }

        throw new InvalidOperationException($"Unsupported AuthDb provider ({dbConfig.Provider})!");
    }

  
}
