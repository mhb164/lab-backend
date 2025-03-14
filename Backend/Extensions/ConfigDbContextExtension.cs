namespace Laboratory.Backend.Extensions;

public static class ConfigDbContextExtension
{
    public static void ConfigDbContext(this IServiceCollection services, ConfigurationManager? configuration)
    {
        services.AddScoped<IBusinessUnitOfWork, BusinessUnitOfWork>();

        var businessDbOptions = BusinessDbOptions.ToModel(configuration?.GetSection(BusinessDbOptions.ConfigName)?.Get<BusinessDbOptions>());
        services.AddScoped(BusinessDbContextFactory(businessDbOptions));
        Log.Information("Business {Provider} configured", businessDbOptions.Provider);

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

        throw new InvalidOperationException($"Unsupported BusinessDb provider ({dbConfig.Provider})!");
    }

}
