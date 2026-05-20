

using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<List<ProductCategory>> GetByBusinessIdAsync(Guid businessId);
        Task<ProductCategory> CreateAsync(ProductCategory category);
        Task DeleteAsync(Guid id);
    }
}
