using LocalMarket.Core.DTos.Common;
using LocalMarket.Core.DTos.Schedule;
using LocalMarket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalMarket.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : BaseController
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // Público — ver horarios de un negocio
        [HttpGet("business/{businessId:guid}")]
        public async Task<IActionResult> GetByBusiness(Guid businessId)
        {
            var result = await _scheduleService.GetByBusinessIdAsync(businessId);
            return Ok(ApiResponseDto<List<ScheduleDto>>.OK(result));
        }

        // Solo dueño del negocio — reemplaza todos los horarios
        [Authorize(Roles = "BusinessOwner")]
        [HttpPut("business/{businessId:guid}")]
        public async Task<IActionResult> Upsert(
            Guid businessId, [FromBody] ScheduleListDto dto)
        {
            var userId = GetUserId();
            var result = await _scheduleService.UpsertAsync(userId, businessId, dto);
            return Ok(ApiResponseDto<List<ScheduleDto>>.OK(
                result, "Schedules updated successfully"));
        }
    }
}
