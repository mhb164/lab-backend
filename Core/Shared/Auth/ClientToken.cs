namespace Laboratory.Auth;

public class ClientToken
{
    public readonly Guid Id;  
    public readonly string Audience;
    public readonly string Issuer;
    public readonly DateTime IssuedAt;
    public readonly DateTime ExpirationTime;

    public ClientToken(Guid? id, string? audience, string? issuer, DateTime? issuedAt, DateTime? expirationTime)
    {
        if (!id.HasValue)
            throw new ArgumentNullException(nameof(issuedAt));

        if (string.IsNullOrWhiteSpace(audience))
            throw new ArgumentException($"{nameof(audience)} is required!", nameof(audience));
        if (string.IsNullOrWhiteSpace(issuer))
            throw new ArgumentException($"{nameof(issuer)} is required!", nameof(issuer));

        if (issuedAt is null)
            throw new ArgumentNullException(nameof(issuedAt));
        if (expirationTime is null)
            throw new ArgumentNullException(nameof(expirationTime));

        Id = id.Value;
        Audience = audience!;
        Issuer = issuer!;
        IssuedAt = issuedAt.Value;
        ExpirationTime = expirationTime.Value;
    }

}
