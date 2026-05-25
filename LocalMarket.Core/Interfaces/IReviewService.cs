
using LocalMarket.Core.DTos.Reviews;

namespace LocalMarket.Core.Interfaces
{
   public interface IReviewService
    {
        Task<List<ReviewDto>> GetByBusinessIdAsync(Guid businessId);
        Task<ReviewDto> CreateAsync(Guid userId, Guid businessId, RequestReviewDto dto);
        Task<ReviewDto> UpdateAsync(Guid userId, Guid reviewId, RequestReviewDto dto);
        Task DeleteAsync(Guid userId, Guid reviewId);
    }
}
