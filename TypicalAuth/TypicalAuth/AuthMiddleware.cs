namespace TypicalAuth;

public class AuthMiddleware
{
    private readonly AuthConfig _config;
    private readonly JwtConfig _jwt;
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next, AuthConfig config, JwtConfig jwtConfig)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _jwt = jwtConfig ?? throw new ArgumentNullException(nameof(jwtConfig));
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserContext userContext)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint is null)
        {
            await _next(context);
            return;
        }

        ((UserContext)userContext).User = GetUser(context);
        context.Items[ClientUser.HttpContextKey] = userContext.User;

        var authMetadata = GetAuthMetadata(endpoint);

        if (!authMetadata.HasAccess(userContext.User?.Permits))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }

    private static AuthMetadata GetAuthMetadata(Endpoint endpoint)
       => endpoint.Metadata?.GetMetadata<AuthMetadata>() ?? AuthMetadata.TokenIsEnough;

    public ClientUser? GetUser(HttpContext context)
    {
        var accessToken = context.GetBearerToken();

        if (string.IsNullOrWhiteSpace(accessToken))
            return default;

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(accessToken, _jwt.ValidationParameters, out var validatedToken);
            var jwtSecurityToken = validatedToken as JwtSecurityToken;

            if (jwtSecurityToken == null)
                return default;

            var tokenId = jwtSecurityToken.GetClaimValue(ClaimNames.TokenId);
            var jwtValues = _config.FromJWT(tokenId, jwtSecurityToken.GetClaimValue(ClaimNames.UserId));

            var token = new ClientToken(id: tokenId, guid: jwtValues.TokenId,
                audience: jwtSecurityToken.Audiences.FirstOrDefault(),
                issuer: jwtSecurityToken.Issuer,
                issuedAt: jwtSecurityToken.IssuedAt,
                expirationTime: jwtSecurityToken.GetClaimTime(ClaimNames.ExpirationTime));

            var auth = new ClientAuth(typeText: jwtSecurityToken.GetClaimValue(ClaimNames.AuthType),
                detail: jwtSecurityToken.GetClaimValue(ClaimNames.AuthDetail),
                time: jwtSecurityToken.GetClaimTime(ClaimNames.AuthTime));

            var user = new ClientUser(token: token, auth,
                id: jwtValues.UserId,
                firstname: jwtSecurityToken.GetClaimValue(ClaimNames.Firstname),
                lastname: jwtSecurityToken.GetClaimValue(ClaimNames.Lastname),
                nickname: jwtSecurityToken.GetClaimValue(ClaimNames.Lastname),
                locallyAvailable: jwtSecurityToken.GetClaimBoolean(ClaimNames.LocallyAvailable),
                changeLocalPasswordRequired: jwtSecurityToken.GetClaimBoolean(ClaimNames.ChangeLocalPasswordRequired),
                readOnly: jwtSecurityToken.GetClaimBoolean(ClaimNames.ReadOnly),
                permits: new UserPermits(ReadPermits(jwtSecurityToken.Claims)));

            return user;
        }
        catch
        {
            return default;
        }
    }



    private static IEnumerable<UserPermit> ReadPermits(IEnumerable<Claim> claims)
    {
        foreach (var claim in claims)
        {
            var permit = UserPermit.FromClaim(claim.Type, claim.Value);

            if (permit is null)
                continue;

            yield return permit;
        }
    }
}