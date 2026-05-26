using LocalMarket.Core.DTos.Business;
using LocalMarket.Core.DTos.Common;
using LocalMarket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LocalMarket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessController : BaseController
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        // Público — listado con filtros opcionales
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] Guid? categoryId,
            [FromQuery] string? city,
            [FromQuery] string? search)
        {
            var result = await _businessService.GetAllAsync(categoryId, city, search);
            return Ok(ApiResponseDto<List<BusinessDto>>.OK(result));
        }

        // Público — detalle de un negocio
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _businessService.GetByIdAsync(id);
            return Ok(ApiResponseDto<BusinessDto>.OK(result));
        }

        // Solo usuarios con rol business
        [Authorize(Roles = "business")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBusinessDto dto)
        {
            var userId = GetUserId();
            var result = await _businessService.CreateAsync(userId, dto);
            return Created($"api/business/{result.Id}",
                ApiResponseDto<BusinessDto>.OK(result, "Business created successfully"));
        }

        // Solo dueño del negocio
        [Authorize(Roles = "business")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBusinessDto dto)
        {
            var userId = GetUserId();
            var result = await _businessService.UpdateAsync(userId, id, dto);
            return Ok(ApiResponseDto<BusinessDto>.OK(result, "Business updated successfully"));
        }

        // Solo dueño del negocio
        [Authorize(Roles = "business")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();
            await _businessService.DeleteAsync(userId, id);
            return Ok(ApiResponseDto<string?>.OK(null, "Business deleted successfully"));
        }
    }
}
