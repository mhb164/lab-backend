namespace Backend;
public interface IUnitOfWork : IDisposable
{
    // Begin a transaction
    void BeginTransaction();

    // Commit all changes within the transaction
    Task<int> CommitAsync(CancellationToken cancellationToken);

    // Rollback the transaction in case of failure
    Task RollbackAsync();
}
