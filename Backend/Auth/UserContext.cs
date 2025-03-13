namespace Laboratory.Backend.Auth;

public class UserContext : IUserContext
{
    public ClientUser? User { get; set; }
}