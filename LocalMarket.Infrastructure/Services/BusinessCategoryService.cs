
using LocalMarket.Core.DTos.Business;
using LocalMarket.Core.Interfaces;
using Mapster;

namespace LocalMarket.Infrastructure.Services
{
    public class BusinessCategoryService : IBusinessCategoryService
    {
        private readonly IBusinessCategoryRepository _repository;

        public BusinessCategoryService(IBusinessCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BusinessCategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Adapt<List<BusinessCategoryDto>>();
        }
    }
}
