using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Backend.Model;

namespace Backend.Repositories;

public class ProductTypesRepository : IProductTypesRepository
{
    private readonly ILogger? _logger;
    private readonly BackendDbContext _context;

    public ProductTypesRepository(ILogger<ProductTypesRepository>? logger, BackendDbContext context)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<ProductType>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.ProductTypes.ToListAsync(cancellationToken);

    public async Task<IEnumerable<ProductType>> GetAllAsync(bool justActive, CancellationToken cancellationToken)
        => justActive
            ? await _context.ProductTypes.Where(x => x.Activation)
                                         .ToListAsync(cancellationToken)
            : await _context.ProductTypes.ToListAsync(cancellationToken);

    public async Task<ProductType?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await _context.ProductTypes.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);


    public async Task<ProductType?> GetByIdAsync(int id, bool justActive, CancellationToken cancellationToken)
        => justActive
            ? await _context.ProductTypes.Where(x => x.Activation)
                                         .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            : await _context.ProductTypes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<ProductType?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        name = name.ToLower();//Ef Core doesn't compile x.Name.Equals Ignore case!
        return await _context.ProductTypes.FirstOrDefaultAsync(x => x.Name.ToLower() == name, cancellationToken);
    }

    public async Task<ProductType?> AddAsync(ProductType item, CancellationToken cancellationToken)
    {
        await _context.ProductTypes.AddAsync(item, cancellationToken);
        return item;
    }

    public async Task<ProductType?> UpdateAsync(ProductType item, CancellationToken cancellationToken)
    {
        _context.ProductTypes.Update(item);
        await Task.CompletedTask;
        return item;
    }
}
