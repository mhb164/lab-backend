namespace TypicalAuth.Interfaces;

public interface IUserTokenRepository
{
    Task<IEnumerable<UserToken>> GetAllAsync(CancellationToken cancellationToken);
    Task<UserToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<UserToken?> AddAsync(UserToken entity, CancellationToken cancellationToken);
    Task<UserToken?> UpdateAsync(UserToken entity, CancellationToken cancellationToken);
    Task DeleteAsync(UserToken entity, CancellationToken cancellationToken);
}
