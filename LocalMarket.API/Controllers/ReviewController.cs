using LocalMarket.Core.DTos.Common;
using LocalMarket.Core.DTos.Reviews;
using LocalMarket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LocalMarket.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : BaseController
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // Público — reseñas de un negocio
        [HttpGet("business/{businessId:guid}")]
        public async Task<IActionResult> GetByBusiness(Guid businessId)
        {
            var result = await _reviewService.GetByBusinessIdAsync(businessId);
            return Ok(ApiResponseDto<List<ReviewDto>>.OK(result));
        }

        // Solo clientes autenticados
        [Authorize(Roles = "Client")]
        [HttpPost("business/{businessId:guid}")]
        public async Task<IActionResult> Create(
            Guid businessId, [FromBody] RequestReviewDto dto)
        {
            var userId = GetUserId();
            var result = await _reviewService.SaveAsync(userId, businessId, dto);
            return Created($"api/review/{result.Id}",
                ApiResponseDto<ReviewDto>.OK(result, "Review created successfully"));
        }

        // Solo el autor de la reseña
        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RequestReviewDto dto)
        {
            var userId = GetUserId();
            var result = await _reviewService.UpdateAsync(userId, id, dto);
            return Ok(ApiResponseDto<ReviewDto>.OK(result, "Review updated successfully"));
        }

        // Solo el autor de la reseña
        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();
            await _reviewService.DeleteAsync(userId, id);
            return Ok(ApiResponseDto<string?>.OK(null, "Review deleted successfully"));
        }

    }
}
