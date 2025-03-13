namespace Laboratory.Backend;

public abstract class UnitOfWork<TContext> where TContext : DbContext, IDisposable
{
    private readonly TContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(TContext  context)
    {
        _context = context;
    }

    // Begin a new transaction
    public void BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    // Commit the transaction to save changes
    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result;
        }

        try
        {
            // Save changes for all repositories
            var result = await _context.SaveChangesAsync(cancellationToken);

            // Commit the transaction (i.e., apply all changes to the database)
            await _transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch (Exception)
        {
            // If anything fails, rollback the transaction
            await RollbackAsync();
            throw; // Rethrow the exception to let the caller handle it
        }
    }

    // Rollback the transaction if something goes wrong
    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
