using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface IBusinessRepository
    {
        Task<List<Business>> GetAllAsync(Guid? categoryId, string? city, string? search);
        Task<Business?> GetByIdAsync(Guid id);
        Task<Business?> GetByUserIdAsync(Guid userId);
        Task<Business> CreateAsync(Business business);
        Task<Business> UpdateAsync(Business business);
        Task DeleteAsync(Business business);
    }
}
