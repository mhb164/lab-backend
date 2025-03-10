using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repositories;

public class UserTokenRepository : IUserTokenRepository
{
    private readonly ILogger? _logger;
    private readonly BackendDbContext _context;

    public UserTokenRepository(ILogger<UserTokenRepository>? logger, BackendDbContext context)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<UserToken>> GetAllAsync(CancellationToken cancellationToken)
       => await _context.UserTokens.ToListAsync(cancellationToken);

    public async Task<UserToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.UserTokens.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<UserToken?> AddAsync(UserToken item, CancellationToken cancellationToken)
    {
        await _context.UserTokens.AddAsync(item, cancellationToken);
        return item;
    }

    public async Task<UserToken?> UpdateAsync(UserToken item, CancellationToken cancellationToken)
    {
        _context.UserTokens.Update(item);
        await Task.CompletedTask;
        return item;
    }

    public async Task DeleteAsync(UserToken item, CancellationToken cancellationToken)
    {
        _context.UserTokens.Remove(item);
        await Task.CompletedTask;
    }
}
