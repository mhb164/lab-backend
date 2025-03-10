using Backend.Dto;

namespace Backend;

public interface IProductTypesService
{
    Task<ServiceResult<IEnumerable<ProductTypeDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<ServiceResult<ProductTypeDto>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ServiceResult<ProductTypeDto>> AddAsync(ProductTypeDto @new, CancellationToken cancellationToken);
    Task<ServiceResult<ProductTypeDto>> UpdateAsync(int id, ProductTypeUpdateDto updated, CancellationToken cancellationToken);
}

