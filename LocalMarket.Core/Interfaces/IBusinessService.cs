using LocalMarket.Core.DTos.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocalMarket.Core.Interfaces
{
    public interface IBusinessService
    {
        Task<List<BusinessDto>> GetAllAsync(Guid? categoryId, string? city, string? search);
        Task<BusinessDto> GetByIdAsync(Guid id);
        Task<BusinessDto> CreateAsync(Guid userId, CreateBusinessDto dto);
        Task<BusinessDto> UpdateAsync(Guid userId, Guid businessId, UpdateBusinessDto dto);
        Task DeleteAsync(Guid userId, Guid businessId);
    }
}
