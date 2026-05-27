using LocalMarket.Core.DTos.Favorites;
using LocalMarket.Core.Entities;
using Mapster;
namespace LocalMarket.Infrastructure.Mappers
{
    public class FavoriteMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Map from Favorite entity to FavoriteDto
            config.NewConfig<Favorite, FavoriteDto>();

            // Map from AddFavoriteDto to Favorite entity
            config.NewConfig<AddFavoriteDto, Favorite>()
                .Map(dest => dest.BusinessId, src => src.BusinessId)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.Id, _ => Guid.NewGuid())
                .Ignore(dest => dest.UserId);
        }
    }
}
