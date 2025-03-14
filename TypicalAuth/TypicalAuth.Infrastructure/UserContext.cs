namespace TypicalAuth;

public class UserContext : IUserContext
{
    public ClientUser? User { get; set; }
}