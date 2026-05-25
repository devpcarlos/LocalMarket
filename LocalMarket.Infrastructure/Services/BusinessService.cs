using LocalMarket.Core.DTos.Business;
using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using Mapster;

namespace LocalMarket.Infrastructure.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository;

        public BusinessService(IBusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
        }

        public async Task<List<BusinessDto>> GetAllAsync(
            string? categoryId, string? city, string? search)
        {
            var businesses = await _businessRepository.GetAllAsync(categoryId, city, search);
            return businesses.Adapt<List<BusinessDto>>();
        }

        public async Task<BusinessDto> GetByIdAsync(Guid id)
        {
            var business = await _businessRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Business {id} not found");

            return business.Adapt<BusinessDto>();
        }

        public async Task<BusinessDto> CreateAsync(Guid userId, CreateBusinessDto dto)
        {
            var existing = await _businessRepository.GetByUserIdAsync(userId);
            if (existing is not null)
                throw new InvalidOperationException(
                    "User already has a registered business");

            var business = dto.Adapt<Business>();
            business.Id = Guid.NewGuid();
            business.UserId = userId;

            var created = await _businessRepository.CreateAsync(business);
            return created.Adapt<BusinessDto>();
        }

        public async Task<BusinessDto> UpdateAsync(
            Guid userId, Guid businessId, UpdateBusinessDto dto)
        {
            var business = await _businessRepository.GetByIdAsync(businessId)
                ?? throw new KeyNotFoundException($"Business {businessId} not found");

            if (business.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this business");

            dto.Adapt(business);
            var updated = await _businessRepository.UpdateAsync(business);
            return updated.Adapt<BusinessDto>();
        }

        public async Task DeleteAsync(Guid userId, Guid businessId)
        {
            var business = await _businessRepository.GetByIdAsync(businessId)
                ?? throw new KeyNotFoundException($"Business {businessId} not found");

            if (business.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this business");

            await _businessRepository.DeleteAsync(businessId);
        }
    }
}
