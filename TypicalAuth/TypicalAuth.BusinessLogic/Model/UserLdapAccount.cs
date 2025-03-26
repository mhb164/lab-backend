namespace TypicalAuth.Model;

public class UserLdapAccount
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Domain { get; set; }

    // Navigation property
    public User User { get; set; } 
}
