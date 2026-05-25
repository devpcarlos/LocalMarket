using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface IBusinessCategoryRepository
    {
        Task<List<BusinessCategory>> GetAllAsync();
        Task<BusinessCategory?> GetByIdAsync(Guid id);
    }
}
