using LocalMarket.Core.DTos.Reviews;
using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using Mapster;

namespace LocalMarket.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {

        private readonly IReviewRepository _reviewRepository;
        private readonly IBusinessRepository _businessRepository;

        public ReviewService(
            IReviewRepository reviewRepository,
            IBusinessRepository businessRepository)
        {
            _reviewRepository = reviewRepository;
            _businessRepository = businessRepository;
        }

        public async Task<List<ReviewDto>> GetByBusinessIdAsync(Guid businessId)
        {
            var reviews = await _reviewRepository.GetByBusinessIdAsync(businessId);
            return reviews.Adapt<List<ReviewDto>>();
        }

        public async Task<ReviewDto> SaveAsync(Guid userId, Guid businessId, RequestReviewDto dto)
        {
            _ = await _businessRepository.GetByIdAsync(businessId)
               ?? throw new KeyNotFoundException($"Business {businessId} not found");

            var existing = await _reviewRepository
                .GetByUserAndBusinessAsync(userId, businessId);

            if (existing is not null)
                throw new InvalidOperationException(
                    "You have already reviewed this business");

            var review = dto.Adapt<Review>();
            review.UserId = userId;
            review.BusinessId = businessId;

            var created = await _reviewRepository.CreateAsync( review);
            return created.Adapt<ReviewDto>();
        }

        public async Task<ReviewDto> UpdateAsync(Guid userId, Guid reviewId, RequestReviewDto dto)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId)
               ?? throw new KeyNotFoundException($"Review {reviewId} not found");

            if (review.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not the author of this review");

            var entity = dto.Adapt(review);

            var updated = await _reviewRepository.UpdateAsync(entity);
            return updated.Adapt<ReviewDto>();
        }

        public async Task DeleteAsync(Guid userId, Guid reviewId)
        {
            // 1. Busca la reseña (Consulta única a la BD)
            var review = await _reviewRepository.GetByIdAsync(reviewId)
               ?? throw new KeyNotFoundException($"Review {reviewId} not found");

            // 2. Valida autoría
            if (review.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not the author of this review");

            // 3. Envía la entidad directo a borrar
            await _reviewRepository.DeleteAsync(review);
        }

    }

}
