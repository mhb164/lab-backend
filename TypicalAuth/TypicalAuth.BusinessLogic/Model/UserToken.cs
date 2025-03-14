namespace TypicalAuth.Model;

public class UserToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required ClientAuthType Type { get; set; }
    public required string Username { get; set; }
    public required DateTime Time { get; set; }
    public required string Description { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime RefereshedAt { get; set; }
}
