namespace Laboratory.Backend.Auth.Interfaces;

public interface IArchivedUserTokenRepository
{
    Task<IEnumerable<ArchivedUserToken>> GetAllAsync(CancellationToken cancellationToken);
    Task<ArchivedUserToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ArchivedUserToken?> AddAsync(ArchivedUserToken entity, CancellationToken cancellationToken);
    Task<ArchivedUserToken?> UpdateAsync(ArchivedUserToken entity, CancellationToken cancellationToken);
}