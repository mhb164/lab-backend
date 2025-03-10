namespace Backend.Auth;

public interface IUserContext
{
    ClientUser? User { get; }
}