namespace TypicalAuth.Model;

public class UserTokenObsolete
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required ClientAuthType Type { get; set; }
    public required string Username { get; set; }
    public required DateTime Time { get; set; }
    public required string Description { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime RefereshedAt { get; set; }

    public required DateTime ObsoleteAt { get; set; }
    public required string Reason { get; set; }

    public static UserTokenObsolete From(UserToken token, DateTime obsoleteAt, string reason)
        => new UserTokenObsolete()
        {
            Id = token.Id,
            UserId = token.UserId,
            Type = token.Type,
            Username = token.Username,
            Time = token.Time,
            Description = token.Description,
            RefreshToken = token.RefreshToken,
            RefereshedAt = token.RefereshedAt,
            ObsoleteAt = obsoleteAt,
            Reason = reason,
        };
}

