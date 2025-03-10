using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repositories;

public class ArchivedUserTokenRepository : IArchivedUserTokenRepository
{
    private readonly ILogger? _logger;
    private readonly BackendDbContext _context;

    public ArchivedUserTokenRepository(ILogger<ArchivedUserTokenRepository>? logger, BackendDbContext context)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<ArchivedUserToken>> GetAllAsync(CancellationToken cancellationToken)
       => await _context.ArchivedUserTokens.ToListAsync(cancellationToken);

    public async Task<ArchivedUserToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.ArchivedUserTokens.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<ArchivedUserToken?> AddAsync(ArchivedUserToken item, CancellationToken cancellationToken)
    {
        await _context.ArchivedUserTokens.AddAsync(item, cancellationToken);
        return item;
    }

    public async Task<ArchivedUserToken?> UpdateAsync(ArchivedUserToken item, CancellationToken cancellationToken)
    {
        _context.ArchivedUserTokens.Update(item);
        await Task.CompletedTask;
        return item;
    }
}
