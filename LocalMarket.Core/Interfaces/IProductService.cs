
using LocalMarket.Core.DTos.Products;

namespace LocalMarket.Core.Interfaces
{
public interface IProductService
    {
        Task<List<ProductDto>> GetByBusinessIdAsync(Guid businessId);
        Task<ProductDto> GetByIdAsync(Guid id);
        Task<ProductDto> CreateAsync(Guid userId, Guid businessId, CreateProductDto dto);
        Task<ProductDto> UpdateAsync(Guid userId, Guid productId, UpdateProductDto dto);
        Task DeleteAsync(Guid userId, Guid productId);
        Task<List<ProductCategoryDto>> GetCategoriesByBusinessIdAsync(Guid businessId);
        Task<ProductCategoryDto> CreateCategoryAsync(Guid userId, Guid businessId, CreateProductCategoryDto dto);
        Task DeleteCategoryAsync(Guid userId, Guid categoryId);
    }
}
