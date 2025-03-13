namespace Laboratory.Backend.Auth.Extensions;

public static class JwtTokenExtensions
{
    public static string? GetClaimValue(this JwtSecurityToken token, string claimType)
    {
        return token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    }

    public static List<string> GetClaimValues(this JwtSecurityToken token, string claimType)
    {
        return token.Claims.Where(c => c.Type == claimType).Select(c => c.Value).ToList();
    }

    public static DateTime? GetClaimTime(this JwtSecurityToken token, string claimType)
    {
        return FromUnixTime(token.GetClaimValue(claimType));
    }

    public static DateTime? FromUnixTime(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (!long.TryParse(value, out var unixTimeSeconds))
            return null;

        return DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds).UtcDateTime;
    }

    public static Guid? GetClaimGuid(this JwtSecurityToken token, string claimType)
    {
        var value = token.GetClaimValue(claimType);
        if (!Guid.TryParse(value, out var guid))
            return null;

        return guid;
    }

    public static bool GetClaimBoolean(this JwtSecurityToken token, string claimType)
    {
        var value = token.GetClaimValue(claimType);
        return "true".Equals(value, StringComparison.OrdinalIgnoreCase);
    }
}