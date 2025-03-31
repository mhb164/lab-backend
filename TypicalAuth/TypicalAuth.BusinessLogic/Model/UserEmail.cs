namespace TypicalAuth.Model;

public class UserEmail
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required string Email { get; set; }
    public required bool Verified { get; set; }

    // Navigation property
    public User User { get; set; }
}
