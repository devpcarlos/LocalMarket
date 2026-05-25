using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetByBusinessIdAsync(Guid businessId);
        Task<Review?> GetByIdAsync(Guid id);
        Task<Review?> GetByUserAndBusinessAsync(Guid userId, Guid businessId);
        Task<Review> CreateAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task DeleteAsync(Guid id);
    }
}
