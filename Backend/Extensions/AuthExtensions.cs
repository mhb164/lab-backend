namespace Laboratory.Backend.Extensions;

public static class AuthExtensions
{
    public static readonly string AdminUsername = "admin";
    public static readonly string TestUsername = "test";
    public static readonly string TestPassword = "test";

    public static IServiceCollection AddTypicalAuth(
        this IServiceCollection services, ILogger? logger, ConfigurationManager? configuration)
    {
        var authDbOptions = AuthDbOptions.ToModel(configuration?.GetSection(AuthDbOptions.ConfigName)?.Get<AuthDbOptions>());
        var authConfig = new AuthConfig();

        var authOptions = configuration?.GetSection(AuthOptions.ConfigName)?.Get<AuthOptions>();
        if (authOptions?.Domains != null)
        {
            foreach (var authOption in authOptions.Domains)
                authConfig.AddDomain(authOption.Displayname, authOption.Name);
        }

        authConfig.Add(UserRole.SuperAdmin,
            new UserPermit("lab", "product_types", new List<string>() { "add", "edit", "remove" }),
            new UserPermit("lab", "user_management", new List<string>() { "change_users_permits" }));

        authConfig.Add(UserRole.Admin,
            new UserPermit("lab", "product_types", new List<string>() { "add", "edit", "remove" }),
            new UserPermit("lab", "user_management", new List<string>()));

        authConfig.Add(UserRole.Operator,
            new UserPermit("lab", "product_types", new List<string>()));

        authConfig.Add(UserRole.View);

        authConfig.ConfigureToJWT((tokenId, userId) =>
        {
            return new(tokenId.ToString("N"), userId.ToString("N"));
        });
        authConfig.ConfigureFromJWT((tokenIdValue, userIdValue) =>
        {
            Guid? tokenId = null;
            if (Guid.TryParse(tokenIdValue, out var parsedTokenId))
                tokenId = parsedTokenId;

            if (!Guid.TryParse(userIdValue, out var userId))
                userId = Guid.Empty;

            return new(tokenId, userId);
        });
        var adminDefaultPassword = configuration?.GetSection($"AdminDefaultPassword")?.Value ?? string.Empty;
        authConfig.Add(new AuthConfig.DefaultUser(AdminUsername, @readonly: true, firstname: "Laboratory", lastname: "Administrator")
            .WithPassword(adminDefaultPassword, true, true)
            .WithRoles(UserRole.Admin));
        authConfig.Add(new AuthConfig.DefaultUser(TestUsername, @readonly: false, firstname: "Laboratory", lastname: "Tester", nickname: "Tester")
            .WithPassword(TestPassword)
            .WithRoles(UserRole.View));

        var jwtConfig = JwtOptions.ToModel(configuration?.GetSection(JwtOptions.ConfigName)?.Get<JwtOptions>());
        services.AddTypicalAuth(config: authConfig, jwtConfig: jwtConfig, authDbContextFactory: AuthDbContextFactory(authDbOptions));
        logger?.LogInformation("Auth {Provider} configured", authDbOptions.Provider);
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
