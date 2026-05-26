using LocalMarket.Core.DTos.Reviews;
using LocalMarket.Core.Entities;
using Mapster;

namespace LocalMarket.Infrastructure.Mappers
{
    public class ReviewMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Map from Review entity to ReviewDto
            config.NewConfig<Review, ReviewDto>();

            // Map from RequestReviewDto to Review entity
            config.NewConfig<RequestReviewDto, Review>()
                .Map(dest => dest.Rating, src => src.Rating)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.Id, _ => Guid.NewGuid())
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.UserId)
                .Ignore(dest => dest.BusinessId);

        }
    }
}
