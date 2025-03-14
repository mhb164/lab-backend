using Microsoft.Extensions.DependencyInjection;
using TypicalAuth.Repositories;

namespace TypicalAuth.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection AddTypicalAuth(this IServiceCollection services, JwtConfig jwtConfig,
       Func<IServiceProvider, AuthDbContext> authDbContextFactory,
       Func<IServiceProvider, AuthService>? authServiceFactory = null,
       Func<IServiceProvider, UserContext>? userContextFactory = null,
       Func<IServiceProvider, PasswordHasher>? passwordHasherFactory = null,
       Func<IServiceProvider, UserRepository>? userRepositoryFactory = null,
       Func<IServiceProvider, UserTokenRepository>? userTokenRepositoryFactory = null,
       Func<IServiceProvider, UserTokenObsoleteRepository>? userTokenObsoleteRepositoryFactory = null,
       Func<IServiceProvider, AuthUnitOfWork>? authUnitOfWorkFactory = null)
    {
        services.AddSingleton(jwtConfig);
        services.AddScoped(authDbContextFactory);

        if (authServiceFactory is null)
            services.AddScoped<IAuthService, AuthService>();
        else
            services.AddScoped(authServiceFactory);

        if (userContextFactory is null)
            services.AddScoped<IUserContext, UserContext>();
        else
            services.AddScoped(userContextFactory);

        if (passwordHasherFactory is null)
            services.AddScoped<IPasswordHasher, PasswordHasher>();
        else
            services.AddScoped(passwordHasherFactory);

        if (userRepositoryFactory is null)
            services.AddScoped<IUserRepository, UserRepository>();
        else
            services.AddScoped(userRepositoryFactory);

        if (userTokenRepositoryFactory is null)
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        else
            services.AddScoped(userTokenRepositoryFactory);

        if (userTokenObsoleteRepositoryFactory is null)
            services.AddScoped<IUserTokenObsoleteRepository, UserTokenObsoleteRepository>();
        else
            services.AddScoped(userTokenObsoleteRepositoryFactory);

        if (authUnitOfWorkFactory is null)
            services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
        else
            services.AddScoped(authUnitOfWorkFactory);

        return services;
    }

    public static async Task<IServiceProvider> WarmUpTypicalAuth(this IServiceProvider services, string adminDefaultPassword)
    {
        using (var scope = services.CreateScope())
        {
            var authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            await authDbContext.InitialAsync();

            var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
            await authService.EnsureDefaultUsersExistsAsync(adminDefaultPassword);
        }
        return services;
    }
}
