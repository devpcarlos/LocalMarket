using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LocalMarket.Infrastructure.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductCategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductCategory>> GetByBusinessIdAsync(Guid businessId)
        {
            return await _dbContext.ProductCategories
                .Where(c => c.BusinessId == businessId)
                .OrderBy(c => c.SortOrder).ToListAsync();
        }

        public async Task<ProductCategory> CreateAsync(ProductCategory category)
        {
            await _dbContext.ProductCategories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _dbContext.ProductCategories.FindAsync(id);
            if (category != null)
            {
                _dbContext.ProductCategories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<ProductCategory?> GetByIdAsync(Guid id)
        {
            return await _dbContext.ProductCategories.FindAsync(id);
        }
    }
}
