using LocalMarket.Core.DTos.Common;
using LocalMarket.Core.DTos.Favorites;
using LocalMarket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LocalMarket.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoriteController : BaseController
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        // Mis favoritos
        [HttpGet]
        public async Task<IActionResult> GetMyFavorites()
        {
            var userId = GetUserId();
            var result = await _favoriteService.GetByUserIdAsync(userId);
            return Ok(ApiResponseDto<List<FavoriteDto>>.OK(result));
        }

        // Agregar favorito
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddFavoriteDto dto)
        {
            var userId = GetUserId();
            var result = await _favoriteService.AddAsync(userId, dto);
            return Created($"api/favorite",
                ApiResponseDto<FavoriteDto>.OK(result, "Business added to favorites"));
        }

        // Quitar favorito por businessId
        [HttpDelete("{businessId:guid}")]
        public async Task<IActionResult> Remove(Guid businessId)
        {
            var userId = GetUserId();
            await _favoriteService.RemoveAsync(userId, businessId);
            return Ok(ApiResponseDto<string?>.OK(null, "Business removed from favorites"));
        }
    }
}
