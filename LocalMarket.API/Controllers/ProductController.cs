using LocalMarket.Core.DTos.Common;
using LocalMarket.Core.DTos.Products;
using LocalMarket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalMarket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Público
        [HttpGet("business/{businessId:guid}")]
        public async Task<IActionResult> GetByBusiness(Guid businessId)
        {
            var result = await _productService.GetByBusinessIdAsync(businessId);
            return Ok(ApiResponseDto<List<ProductDto>>.OK(result));
        }

        // Público
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _productService.GetByIdAsync(id);
            return Ok(ApiResponseDto<ProductDto>.OK(result));
        }

        // Solo dueño del negocio
        [Authorize(Roles = "BusinessOwner")]
        [HttpPost("business/{businessId:guid}")]
        public async Task<IActionResult> Create(
            Guid businessId, [FromBody] RequestProductDto dto)
        {
            var userId = GetUserId();
            var result = await _productService.CreateAsync(userId, businessId, dto);
            return Created($"api/product/{result.Id}",
                ApiResponseDto<ProductDto>.OK(result, "Product created successfully"));
        }

        // Solo dueño del negocio
        [Authorize(Roles = "BusinessOwner")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id, [FromBody]  RequestProductDto dto)
        {
            var userId = GetUserId();
            var result = await _productService.UpdateAsync(userId, id, dto);
            return Ok(ApiResponseDto<ProductDto>.OK(result, "Product updated successfully"));
        }

        // Solo dueño del negocio
        [Authorize(Roles = "BusinessOwner")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();
            await _productService.DeleteAsync(userId, id);
            return Ok(ApiResponseDto<string?>.OK(null, "Product deleted successfully"));
        }

        // Categorías
        [HttpGet("categories/business/{businessId:guid}")]
        public async Task<IActionResult> GetCategories(Guid businessId)
        {
            var result = await _productService
                .GetCategoriesByBusinessIdAsync(businessId);
            return Ok(ApiResponseDto<List<ProductCategoryDto>>.OK(result));
        }

        [Authorize(Roles = "BusinessOwner")]
        [HttpPost("categories/business/{businessId:guid}")]
        public async Task<IActionResult> CreateCategory(
            Guid businessId, [FromBody] CreateProductCategoryDto dto)
        {
            var userId = GetUserId();
            var result = await _productService
                .CreateCategoryAsync(userId, businessId, dto);
            return Created($"api/product/categories/{result.Id}",
                ApiResponseDto<ProductCategoryDto>.OK(
                    result, "Category created successfully"));
        }

        [Authorize(Roles = "BusinessOwner")]
        [HttpDelete("categories/{categoryId:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var userId = GetUserId();
            await _productService.DeleteCategoryAsync(userId, categoryId);
            return Ok(ApiResponseDto<string?>.OK(null, "Category deleted successfully"));
        }
    }
}
