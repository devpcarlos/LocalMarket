using LocalMarket.Core.DTos.Products;
using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using Mapster;

namespace LocalMarket.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IBusinessRepository _businessRepository;

        public ProductService(
            IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository,
            IBusinessRepository businessRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _businessRepository = businessRepository;
        }

        public async Task<List<ProductDto>> GetByBusinessIdAsync(Guid businessId)
        {
            var products = await _productRepository.GetByBusinessIdAsync(businessId);
            return products.Adapt<List<ProductDto>>();
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Product {id} not found");

            return product.Adapt<ProductDto>();
        }

        public async Task<ProductDto> CreateAsync(
            Guid userId, Guid businessId, RequestProductDto dto)
        {
            var business = await _businessRepository.GetByIdAsync(businessId)
                ?? throw new KeyNotFoundException($"Business {businessId} not found");

            if (business.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this business");

            var product = dto.Adapt<Product>();
            product.Id = Guid.NewGuid();
            product.BusinessId = businessId;

            var created = await _productRepository.CreateAsync(product);
            return created.Adapt<ProductDto>();
        }

        public async Task<ProductDto> UpdateAsync(
            Guid userId, Guid productId, RequestProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new KeyNotFoundException($"Product {productId} not found");

            var business = await _businessRepository.GetByIdAsync(product.BusinessId)
                ?? throw new KeyNotFoundException("Business not found");

            if (business.UserId != userId)
                throw new UnauthorizedAccessException(nameof(userId));

            dto.Adapt(product);
            var updated = await _productRepository.UpdateAsync(product);
            return updated.Adapt<ProductDto>();
        }

        public async Task DeleteAsync(Guid userId, Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new KeyNotFoundException($"Product {productId} not found");

            var business = await _businessRepository.GetByIdAsync(product.BusinessId)
                ?? throw new KeyNotFoundException("Business not found");

            if (business.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this business");

            await _productRepository.DeleteAsync(productId);
        }

        public async Task<List<ProductCategoryDto>> GetCategoriesByBusinessIdAsync(
            Guid businessId)
        {
            var categories = await _productCategoryRepository
                .GetByBusinessIdAsync(businessId);

            return categories.Adapt<List<ProductCategoryDto>>();
        }

        public async Task<ProductCategoryDto> CreateCategoryAsync(
            Guid userId, Guid businessId, CreateProductCategoryDto dto)
        {
            var business = await _businessRepository.GetByIdAsync(businessId)
                ?? throw new KeyNotFoundException($"Business {businessId} not found");

            if (business.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this business");

            var category = dto.Adapt<ProductCategory>();
            category.Id = Guid.NewGuid();
            category.BusinessId = businessId;

            var created = await _productCategoryRepository.CreateAsync(category);
            return created.Adapt<ProductCategoryDto>();
        }

        public async Task DeleteCategoryAsync(Guid userId, Guid categoryId)
        {
            var category = await _productCategoryRepository.GetByIdAsync(categoryId)
                ?? throw new KeyNotFoundException($"Category {categoryId} not found");

            var business = await _businessRepository.GetByIdAsync(category.BusinessId)
                ?? throw new KeyNotFoundException("Business not found");

            if (business.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this business");

            await _productCategoryRepository.DeleteAsync(categoryId);
        }
    }
}
