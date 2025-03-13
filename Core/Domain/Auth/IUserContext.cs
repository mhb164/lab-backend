namespace Laboratory.Backend.Auth;

public interface IUserContext
{
    ClientUser? User { get; }
}