namespace Laboratory.Backend.Auth.Config;

public class JwtConfig
{
    public readonly string Issuer;
    public readonly SymmetricSecurityKey SecretKey;
    public readonly TokenValidationParameters ValidationParameters;
    public readonly int LifetimeInMinutes;

    public JwtConfig(string? issuer, string? secret, int? lifetimeInMinutes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(issuer);
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);
        ArgumentNullException.ThrowIfNull(lifetimeInMinutes);

        Issuer = issuer.Trim();
        var jwtSecretBytes = Encoding.UTF8.GetBytes(secret.Trim());
        SecretKey = new SymmetricSecurityKey(jwtSecretBytes);
        LifetimeInMinutes = lifetimeInMinutes.Value;

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
