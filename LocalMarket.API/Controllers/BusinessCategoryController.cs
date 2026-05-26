using LocalMarket.Core.DTos.Business;
using LocalMarket.Core.DTos.Common;
using LocalMarket.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalMarket.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BusinessCategoryController : ControllerBase
    {
        private readonly IBusinessCategoryService _service;

        public BusinessCategoryController(IBusinessCategoryService service)
        {
            _service = service;
        }

        // Público — lista todas las categorías de negocio
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponseDto<List<BusinessCategoryDto>>.OK(result));
        }
    }
}
