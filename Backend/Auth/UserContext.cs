using Backend.Interfaces;

namespace Backend.Auth;

public class UserContext : IUserContext
{
    public ClientUser? User { get; set; }
}