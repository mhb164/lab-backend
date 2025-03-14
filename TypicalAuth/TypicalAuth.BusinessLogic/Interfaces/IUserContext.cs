namespace TypicalAuth.Interfaces;

public interface IUserContext
{
    ClientUser? User { get; }
}