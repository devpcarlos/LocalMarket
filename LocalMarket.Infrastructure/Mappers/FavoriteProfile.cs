using LocalMarket.Core.DTos.Favorites;
using LocalMarket.Core.Entities;
using Mapster;
namespace LocalMarket.Infrastructure.Mappers
{
    public class FavoriteProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Map from Favorite entity to FavoriteDto
            config.NewConfig<Favorite, FavoriteDto>()
                .Map(dest => dest.BusinessId, src => src.BusinessId)
                .Map(dest => dest.UserId, src => src.UserId);

            // Map from AddFavoriteDto to Favorite entity
            config.NewConfig<FavoriteDto, Favorite>()
                .Map(dest => dest.BusinessId, src => src.BusinessId)
                .Map(dest => dest.UserId, src => src.UserId);
        }
    }
}
