namespace TypicalAuth.Interfaces;

public interface IAuthUnitOfWork : IDisposable
{
    // Begin a transaction
    void BeginTransaction();

    // Commit all changes within the transaction
    Task<int> CommitAsync(CancellationToken cancellationToken);

    // Rollback the transaction in case of failure
    Task RollbackAsync();
}
