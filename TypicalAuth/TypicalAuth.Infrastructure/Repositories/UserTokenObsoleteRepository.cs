namespace TypicalAuth.Repositories;

public class UserTokenObsoleteRepository : IUserTokenObsoleteRepository
{
    private readonly ILogger? _logger;
    private readonly AuthDbContext _context;

    public UserTokenObsoleteRepository(ILogger<UserTokenObsoleteRepository>? logger, AuthDbContext context)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<UserTokenObsolete>> GetAllAsync(CancellationToken cancellationToken)
       => await _context.UserTokenHistory.ToListAsync(cancellationToken);

    public async Task<UserTokenObsolete?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.UserTokenHistory.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<UserTokenObsolete?> AddAsync(UserTokenObsolete item, CancellationToken cancellationToken)
    {
        await _context.UserTokenHistory.AddAsync(item, cancellationToken);
        return item;
    }

    public async Task<UserTokenObsolete?> UpdateAsync(UserTokenObsolete item, CancellationToken cancellationToken)
    {
        _context.UserTokenHistory.Update(item);
        await Task.CompletedTask;
        return item;
    }
}
