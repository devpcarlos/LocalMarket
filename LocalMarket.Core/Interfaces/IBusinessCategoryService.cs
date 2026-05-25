
using LocalMarket.Core.DTos.Business;

namespace LocalMarket.Core.Interfaces
{
    public interface IBusinessCategoryService
    {
        Task<List<BusinessCategoryDto>> GetAllAsync();

    }
}
