namespace Laboratory.Backend.Interfaces;

public interface IRepository<TEntity, TId>
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}


public interface IRepositoryWithDelete<TEntity, TId> : IRepository<TEntity, TId>
{
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
}

public interface IProductTypesRepository : IRepository<ProductType, int>
{
    Task<IEnumerable<ProductType>> GetAllAsync(bool justActive, CancellationToken cancellationToken);
    Task<ProductType?> GetByIdAsync(int id, bool justActive, CancellationToken cancellationToken);
    Task<ProductType?> GetByNameAsync(string name, CancellationToken cancellationToken);
}
