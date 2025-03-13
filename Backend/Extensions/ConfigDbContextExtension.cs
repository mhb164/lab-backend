namespace Laboratory.Backend.Extensions;

public static class ConfigDbContextExtension
{
    public static void ConfigDbContext(this IServiceCollection services, ConfigurationManager? configuration)
    {
        services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
        services.AddScoped<IBusinessUnitOfWork, BusinessUnitOfWork>();

        var authDbOptions = AuthDbOptions.ToModel(configuration?.GetSection(AuthDbOptions.ConfigName)?.Get<AuthDbOptions>());
        services.AddScoped(AuthDbContextFactory(authDbOptions));
        Log.Information("Auth {Provider} configured", authDbOptions.Provider);


        var businessDbOptions = BusinessDbOptions.ToModel(configuration?.GetSection(BusinessDbOptions.ConfigName)?.Get<BusinessDbOptions>());
        services.AddScoped(BusinessDbContextFactory(businessDbOptions));
        Log.Information("Business {Provider} configured", authDbOptions.Provider);

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

    private static Func<IServiceProvider, BusinessDbContext> BusinessDbContextFactory(DbConfig dbConfig)
    {
        if (dbConfig.IsSqlServer)
        {
            return (provider) =>
            {
                var logger = provider.GetRequiredService<ILogger<BusinessSqlDbContext>>();
                return new BusinessSqlDbContext(logger, dbConfig.ConnectionString);
            };
        }

        if (dbConfig.IsSqlite)
        {
            return (provider) =>
            {
                var logger = provider.GetRequiredService<ILogger<BusinessSqliteDbContext>>();
                return new BusinessSqliteDbContext(logger, dbConfig.ConnectionString);
            };
        }

        throw new InvalidOperationException($"Unsuported BusinessDb provider ({dbConfig.Provider})!");
    }

}
