namespace Laboratory.Backend.Auth;

public sealed class AuthUnitOfWork : UnitOfWork<AuthDbContext>, IAuthUnitOfWork
{
    public AuthUnitOfWork(AuthDbContext context) : base(context)
    {
    }
}
