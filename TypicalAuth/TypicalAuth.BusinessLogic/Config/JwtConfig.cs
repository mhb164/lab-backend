namespace TypicalAuth.Config;

public sealed class JwtConfig
{
    public readonly string Issuer;
    public readonly SymmetricSecurityKey SecretKey;
    public readonly TokenValidationParameters ValidationParameters;
    public readonly double LifetimeInMinutes;
    public readonly double RefreshInDays;

    public JwtConfig(string? issuer, string? secret, double? lifetimeInMinutes, double? refreshInDays)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(issuer);
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);
        ArgumentNullException.ThrowIfNull(lifetimeInMinutes);
        ArgumentNullException.ThrowIfNull(refreshInDays);

        Issuer = issuer.Trim();
        var jwtSecretBytes = System.Text.Encoding.UTF8.GetBytes(secret.Trim());
        SecretKey = new SymmetricSecurityKey(jwtSecretBytes);
        LifetimeInMinutes = lifetimeInMinutes.Value;
        RefreshInDays = refreshInDays.Value;

        ValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = [SecretKey],
            ValidateLifetime = true
        };
    }
}
