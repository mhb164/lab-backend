namespace Laboratory.Backend;

public static class HttpContextExtension
{
    public static readonly string BearerAuthorizationStart = "Bearer ";
    public static readonly string RefreshTokenHeaderName = "refresh_token";

    public static ClientUser? GetClientUser(this HttpContext httpContext) => httpContext?.Items[ClientUser.HttpContextKey] as ClientUser;

    public static TokenDto? GetToken(this HttpContext context)
    {
        var refreshToken = context.GetRefreshToken();
        if (string.IsNullOrWhiteSpace(refreshToken))
            return default;

        var accessToken = context.GetBearerToken();
        if (string.IsNullOrWhiteSpace(accessToken))
            return default;

        return new TokenDto() { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public static string? GetBearerToken(this HttpContext context)
    {
        var authHeader = context?.Request?.Headers[HeaderNames.Authorization].FirstOrDefault();

        if (authHeader is null)
            return null;

        if (!authHeader.StartsWith(BearerAuthorizationStart))
            return null;

        return (string?)authHeader[BearerAuthorizationStart.Length..].Trim();
    }

    public static string? GetRefreshToken(this HttpContext context)
    {
        var refreshTokenHeader = context?.Request?.Headers[RefreshTokenHeaderName].FirstOrDefault();

        if (refreshTokenHeader is null)
            return null;

        return refreshTokenHeader.Trim();
    }
}
