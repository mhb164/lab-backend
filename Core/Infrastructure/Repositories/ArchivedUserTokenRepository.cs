namespace Laboratory.Backend.Repositories;

public class ArchivedUserTokenRepository : IArchivedUserTokenRepository
{
    private readonly ILogger? _logger;
    private readonly AuthDbContext _context;

    public ArchivedUserTokenRepository(ILogger<ArchivedUserTokenRepository>? logger, AuthDbContext context)
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
