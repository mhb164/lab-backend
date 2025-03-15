namespace Shared.Model;

public class ClientToken
{
    public readonly string Id;  
    public readonly string Audience;
    public readonly string Issuer;
    public readonly DateTime IssuedAt;
    public readonly DateTime ExpirationTime;

    public readonly Guid? GUID;

    public ClientToken(string? id, Guid? guid,  string? audience, string? issuer, DateTime? issuedAt, DateTime? expirationTime)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException($"{nameof(id)} is required!", nameof(id));

        if (string.IsNullOrWhiteSpace(audience))
            throw new ArgumentException($"{nameof(audience)} is required!", nameof(audience));
        if (string.IsNullOrWhiteSpace(issuer))
            throw new ArgumentException($"{nameof(issuer)} is required!", nameof(issuer));

        if (issuedAt is null)
            throw new ArgumentNullException(nameof(issuedAt));
        if (expirationTime is null)
            throw new ArgumentNullException(nameof(expirationTime));

        Id = id!;
        if (!guid.HasValue && Guid.TryParse(Id, out var parsedId))
            GUID = parsedId;

        Audience = audience!;
        Issuer = issuer!;
        IssuedAt = issuedAt.Value;
        ExpirationTime = expirationTime.Value;
    }

}
