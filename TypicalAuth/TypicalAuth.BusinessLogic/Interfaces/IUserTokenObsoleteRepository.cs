namespace TypicalAuth.Interfaces;

public interface IUserTokenObsoleteRepository 
{
    Task<IEnumerable<UserTokenObsolete>> GetAllAsync(CancellationToken cancellationToken);
    Task<UserTokenObsolete?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<UserTokenObsolete?> AddAsync(UserTokenObsolete entity, CancellationToken cancellationToken);
    Task<UserTokenObsolete?> UpdateAsync(UserTokenObsolete entity, CancellationToken cancellationToken);
}