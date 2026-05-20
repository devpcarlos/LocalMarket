using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence.Models;
using Mapster;

namespace LocalMarket.Infrastructure.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly Supabase.Client _supabase;

        public ProductCategoryRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<ProductCategory>> GetByBusinessIdAsync(Guid businessId)
        {
            var result = await _supabase
                .From<ProductCategoryModel>()
                .Where(c => c.BusinessId == businessId)
                .Order(c => c.SortOrder, Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();

            return result.Models.Adapt<List<ProductCategory>>();
        }

        public async Task<ProductCategory> CreateAsync(ProductCategory category)
        {
            var model = category.Adapt<ProductCategoryModel>();
            var result = await _supabase
                .From<ProductCategoryModel>()
                .Insert(model);

            return result.Models.First().Adapt<ProductCategory>();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _supabase
                .From<ProductCategoryModel>()
                .Where(c => c.Id == id)
                .Delete();
        }
    }
}
