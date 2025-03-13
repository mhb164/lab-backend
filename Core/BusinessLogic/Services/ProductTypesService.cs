namespace Laboratory.Backend.Services;

public class ProductTypesService : IProductTypesService
{
    private readonly ILogger? _logger;
    private readonly IBusinessUnitOfWork _unitOfWork;
    private readonly IProductTypesRepository _repository;

    public ProductTypesService(ILogger<ProductTypesService>? logger, IBusinessUnitOfWork unitOfWork, IProductTypesRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<ServiceResult<IEnumerable<ProductTypeDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var serviceResult = new ServiceResult<IEnumerable<ProductTypeDto>>();

        try
        {
            var items = await _repository.GetAllAsync(cancellationToken);
            return serviceResult.Success(items.ToDto());
        }
        catch (Exception ex)
        {
            var trackingId = Guid.NewGuid();
            _logger?.LogError(ex, "[{TrackingId}] An error occurred while retrieving product types.", trackingId);
            return serviceResult.InternalError(trackingId, "Database error occurred.");
        }
    }

    public async Task<ServiceResult<ProductTypeDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var serviceResult = new ServiceResult<ProductTypeDto>();

        try
        {
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing is null)
                return serviceResult.NotFound($"{nameof(ProductTypeDto)} with Id {id} not found.");

            return serviceResult.Success(existing.ToDto());
        }
        catch (Exception ex)
        {
            var trackingId = Guid.NewGuid();
            _logger?.LogError(ex, "[{TrackingId}] An error occurred while retrieving product type [Id:{Id}].", trackingId, id);
            return serviceResult.InternalError(trackingId, "Database error occurred.");
        }
    }

    public async Task<ServiceResult<ProductTypeDto>> AddAsync(ProductTypeDto @new, CancellationToken cancellationToken)
    {
        var serviceResult = new ServiceResult<ProductTypeDto>();
        _unitOfWork.BeginTransaction();

        try
        {
            if (@new == null)
                return serviceResult.BadRequest($"Invalid request. {nameof(ProductTypeDto)} cannot be null.");

            if (string.IsNullOrWhiteSpace(@new.Name))
                return serviceResult.BadRequest($"{nameof(ProductType)} Name cannot be empty.");

            var existingByName = await _repository.GetByNameAsync(@new.Name, cancellationToken);
            if (existingByName != null && existingByName.Id != @new.Id)
            {
                return serviceResult.Conflict($"A {nameof(ProductType)} with the name '{@new.Name}' already exists.");
            }

            var added = await _repository.AddAsync(@new.ToModel(), cancellationToken);
            var changeCount = await _unitOfWork.CommitAsync(cancellationToken);

            if (added is null || changeCount <= 0)
                throw new InvalidOperationException("Nothing changed!");

            return ServiceResult.Success(added.ToDto());
        }
        catch (Exception ex)
        {
            var trackingId = Guid.NewGuid();
            _logger?.LogError(ex, "[{TrackingId}] Database error while adding ProductType.", trackingId);
            await _unitOfWork.RollbackAsync();
            return serviceResult.InternalError(trackingId, "Database error occurred.");
        }
    }

    public async Task<ServiceResult<ProductTypeDto>> UpdateAsync(int id, ProductTypeUpdateDto request, CancellationToken cancellationToken)
    {
        var serviceResult = new ServiceResult<ProductTypeDto>();

        _unitOfWork.BeginTransaction();

        try
        {
            if (request == null)
                return serviceResult.BadRequest($"Invalid request. {nameof(ProductType)} cannot be null.");

            if (string.IsNullOrWhiteSpace(request.Name))
                return serviceResult.BadRequest($"{nameof(ProductType)} Name cannot be empty.");

            var existing = await _repository.GetByIdAsync(id, true, cancellationToken);
            if (existing is null)
                return serviceResult.NotFound($"{nameof(ProductType)} with Id {id} not found.");

            var existingByName = await _repository.GetByNameAsync(request.Name, cancellationToken);
            if (existingByName != null && existingByName.Id != existing.Id)
            {
                return serviceResult.Conflict($"A {nameof(ProductType)} with the name '{request.Name}' already exists.");
            }

            // Update only necessary properties
            existing.Name = request.Name;
            existing.Activation = request.Activation;

            var updatedOnRepo = await _repository.UpdateAsync(existing, cancellationToken);
            var changeCount = await _unitOfWork.CommitAsync(cancellationToken);

            if (updatedOnRepo is null || changeCount <= 0)
                throw new InvalidOperationException("Nothing changed!");

            return ServiceResult.Success(updatedOnRepo.ToDto());
        }
        catch (Exception ex)
        {
            var trackingId = Guid.NewGuid();
            _logger?.LogError(ex, "[{TrackingId}] Database error while updating ProductType.", trackingId);
            await _unitOfWork.RollbackAsync();
            return serviceResult.InternalError(trackingId, "Database error occurred.");
        }
    }
}
