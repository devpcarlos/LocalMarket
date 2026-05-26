using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LocalMarket.Infrastructure.Repositories
{
    public class BusinessCategoryRepository : IBusinessCategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public BusinessCategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BusinessCategory>> GetAllAsync()
        {
            return await _dbContext.BusinessCategories
                .AsNoTracking()
                .OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<BusinessCategory?> GetByIdAsync(Guid id)
        {
            return await _dbContext.BusinessCategories.FindAsync(id);
        }
    }
}
