using LocalMarket.Core.DTos.Common;
using LocalMarket.Core.DTos.Subscriptions;
using LocalMarket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalMarket.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionController : BaseController
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        // Negocio — solicitar suscripción (después de pagar por Nequi)
        [HttpPost("business/{businessId:guid}")]
        public async Task<IActionResult> Request(
            Guid businessId, [FromBody] CreateSubscriptionDto dto)
        {
            var userId = GetUserId();
            var result = await _subscriptionService.RequestAsync(userId, businessId, dto);
            return Created($"api/subscription/{result.Id}",
                ApiResponseDto<SubscriptionDto>.OK(result,
                    "Subscription requested. Pending admin activation."));
        }

        // Admin — activar suscripción pendiente
        [HttpPatch("activate")]
        public async Task<IActionResult> Activate([FromBody] ActivateSubscriptionDto dto)
        {
            var adminId = GetUserId();
            var result = await _subscriptionService.ActivateAsync(adminId, dto);
            return Ok(ApiResponseDto<SubscriptionDto>.OK(
                result, "Subscription activated successfully"));
        }

        // Público — ver suscripción activa de un negocio
        [AllowAnonymous]
        [HttpGet("business/{businessId:guid}/active")]
        public async Task<IActionResult> GetActive(Guid businessId)
        {
            var result = await _subscriptionService
                .GetActiveByBusinessIdAsync(businessId);

            return Ok(ApiResponseDto<SubscriptionDto?>.OK(result));
        }

        // Negocio — ver historial de suscripciones propias
        [HttpGet("business/{businessId:guid}")]
        public async Task<IActionResult> GetByBusiness(Guid businessId)
        {
            var result = await _subscriptionService
                .GetByBusinessIdAsync(businessId);

            return Ok(ApiResponseDto<List<SubscriptionDto>>.OK(result));
        }

        // Admin — ver suscripciones pendientes de activar
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var adminId = GetUserId();
            var result = await _subscriptionService.GetPendingAsync();
            return Ok(ApiResponseDto<List<SubscriptionDto>>.OK(result));
        }
    }
}
