﻿namespace TypicalAuth.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> AddAsync(User entity, CancellationToken cancellationToken);
    Task<User?> UpdateAsync(User entity, CancellationToken cancellationToken);
    Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<User?> GetByLdapAsync(string username, string domainName, CancellationToken cancellationToken);
}
