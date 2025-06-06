﻿namespace TypicalAuth.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints, string? pathPrefix = null)
    {
        if (string.IsNullOrWhiteSpace(pathPrefix))
            pathPrefix = string.Empty;
        else if (!pathPrefix.StartsWith('/'))
            pathPrefix = $"/{pathPrefix}";

        endpoints.MapGet($"{pathPrefix}/sign-in-options",
            (IAuthService service)
                => Results.Ok(service.GetSignInOptions()))
            .WithMetadata(AuthMetadata.Public);

        endpoints.MapPost($"{pathPrefix}/sign-in",
            async ([FromBody] SignInRequest request, IAuthService service, CancellationToken cancellationToken)
                => await service.SignInAsync(request, cancellationToken).MapAsync())
            .WithMetadata(AuthMetadata.Public);

        endpoints.MapPost($"{pathPrefix}/change-local-password",
           async ([FromBody] ChangeLocalPasswordRequest request, IAuthService service, CancellationToken cancellationToken)
               => await service.ChangeLocalPasswordRequestAsync(request, cancellationToken).MapAsync())
           .WithMetadata(AuthMetadata.TokenIsEnough);

        endpoints.MapPost($"{pathPrefix}/refresh-token",
            async (HttpContext httpContext, IAuthService service, CancellationToken cancellationToken)
                => await service.RefreshTokenAsync(httpContext.GetRefreshToken(), cancellationToken).MapAsync())
            .WithMetadata(AuthMetadata.Public);

        endpoints.MapPost($"{pathPrefix}/sign-out",
            async (IAuthService service, CancellationToken cancellationToken)
                => await service.SignOutAsync(cancellationToken).MapAsync())
            .WithMetadata(AuthMetadata.TokenIsEnough);

        endpoints.MapGet($"{pathPrefix}/user-info",
            async (IAuthService service, CancellationToken cancellationToken)
               => await service.GetUserInfoAsync(cancellationToken).MapAsync())
           .WithMetadata(AuthMetadata.TokenIsEnough);

        return endpoints;
    }
}
