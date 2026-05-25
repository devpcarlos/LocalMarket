using LocalMarket.Core.DTos.Favorites;
using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using Mapster;

namespace LocalMarket.Infrastructure.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IBusinessRepository _businessRepository;

        public FavoriteService(
            IFavoriteRepository favoriteRepository,
            IBusinessRepository businessRepository)
        {
            _favoriteRepository = favoriteRepository;
            _businessRepository = businessRepository;
        }

        public async Task<List<FavoriteDto>> GetByUserIdAsync(Guid userId)
        {
            var  favorites= await _favoriteRepository.GetByUserIdAsync(userId);
            return favorites.Adapt<List<FavoriteDto>>();
        }

        public async Task<FavoriteDto> AddAsync(Guid userId, FavoriteDto dto)
        {
            _ = await _businessRepository.GetByIdAsync(dto.BusinessId)
               ?? throw new KeyNotFoundException($"Business {dto.BusinessId} not found");

            var existing = await _favoriteRepository
                .GetByUserAndBusinessAsync(userId, dto.BusinessId);

            if (existing is not null)
                throw new InvalidOperationException("Business is already in favorites");
            
            var favorite = dto.Adapt<Favorite>();
            await _favoriteRepository.CreateAsync(favorite);

            return favorite.Adapt<FavoriteDto>();
        }

       
        public async Task RemoveAsync(Guid userId, Guid businessId)
        {
            var favorite = await _favoriteRepository
                .GetByUserAndBusinessAsync(userId, businessId)
                ?? throw new KeyNotFoundException("Favorite not found");

            if (favorite.UserId != userId)
                throw new UnauthorizedAccessException("You cannot remove this favorite");

            await _favoriteRepository.DeleteAsync(favorite.Id);
        }
    }
}
