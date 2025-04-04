﻿using System.Xml.Linq;

namespace TypicalAuth.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ILogger? _logger;
    private readonly AuthDbContext _context;

    public UserRepository(ILogger<UserRepository>? logger, AuthDbContext context)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
       => await _context.Users.ToListAsync(cancellationToken);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Users
            .Include(u => u.LdapAccounts)
            .Include(u => u.Emails)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        name = name.ToLower();//Ef Core doesn't compile x.Name.Equals Ignore case!
        return await _context.Users
            .Include(u => u.LdapAccounts)
            .Include(u => u.Emails)
            .FirstOrDefaultAsync(u => u.Username.ToLower() == name, cancellationToken);
    }

    public async Task<User?> GetByLdapAsync(string username, string domainName, CancellationToken cancellationToken)
    {
        var userLdapAccount = await _context.UserLdapAccounts
            .FirstOrDefaultAsync(ula => ula.Username.ToLower() == username && ula.Domain.ToLower() == domainName, cancellationToken);

        if (userLdapAccount == null)
            return null;

        return await GetByIdAsync(userLdapAccount.UserId, cancellationToken);
    }


    public async Task<User?> AddAsync(User item, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(item, cancellationToken);
        return item;
    }

    public async Task<User?> UpdateAsync(User item, CancellationToken cancellationToken)
    {
        _context.Users.Update(item);
        await Task.CompletedTask;
        return item;
    }
}
