

using LocalMarket.Core.DTos.Subscriptions;
using LocalMarket.Core.Entities;
using LocalMarket.Core.Enums;
using LocalMarket.Core.Interfaces;
using Mapster;

namespace LocalMarket.Infrastructure.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IUserRepository _userRepository;

        // Precios definidos en el servidor — nunca los envía el cliente
        private static readonly Dictionary<SubscriptionPlan, decimal> Prices = new()
        {
            { SubscriptionPlan.Basic, 25000m },
            { SubscriptionPlan.Standard, 50000m },
            { SubscriptionPlan.Premium, 100000m }
        };

        public SubscriptionService(
           ISubscriptionRepository subscriptionRepository,
           IBusinessRepository businessRepository,
           IUserRepository userRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _businessRepository = businessRepository;
            _userRepository = userRepository;
        }

        public async Task<SubscriptionDto> RequestAsync(Guid userId, Guid businessId, CreateSubscriptionDto dto)
        {
            var business = await _businessRepository.GetByIdAsync(businessId)
                ?? throw new KeyNotFoundException($"Business {businessId} not found");

            if (business.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this business");

            var active = await _subscriptionRepository
                .GetActiveByBusinessIdAsync(businessId);

            if (active is not null)
                throw new InvalidOperationException(
                    "This business already has an active subscription");

            var plan = Enum.Parse<SubscriptionPlan>(dto.Plan, ignoreCase: true);
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var subscription = new Subscription
            {
                Id = Guid.NewGuid(),
                BusinessId = businessId,
                Plan = plan,
                MonthlyPrice = Prices[plan],
                StartDate = today,
                EndDate = today.AddMonths(1),
                IsActive = false,
                PaymentDescription = dto.PaymentDescription,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _subscriptionRepository.CreateAsync(subscription);
            return created.Adapt<SubscriptionDto>();
        }
        public async Task<SubscriptionDto> ActivateAsync(Guid adminId, ActivateSubscriptionDto dto)
        {
            var admin = await _userRepository.GetByIdAsync(adminId)
               ?? throw new KeyNotFoundException("Admin user not found");

            if (admin.Role != UserRole.Admin)
                throw new UnauthorizedAccessException(
                    "Only admins can activate subscriptions");

            var subscription = await _subscriptionRepository.GetByIdAsync(dto.SubscriptionId)
                ?? throw new KeyNotFoundException(
                    $"Subscription {dto.SubscriptionId} not found");

            if (subscription.IsActive)
                throw new InvalidOperationException(
                    "This subscription is already active");

            subscription.IsActive = true;
            subscription.ActivatedBy = adminId;
            subscription.ActivatedAt = DateTime.UtcNow;

            var updated = await _subscriptionRepository.UpdateAsync(subscription);
            return updated.Adapt<SubscriptionDto>();
        }

        public async Task<SubscriptionDto?> GetActiveByBusinessIdAsync(Guid businessId)
        {
            var subscription = await _subscriptionRepository
                .GetActiveByBusinessIdAsync(businessId);

            return subscription?.Adapt<SubscriptionDto>();
        }

        public async Task<List<SubscriptionDto>> GetByBusinessIdAsync(Guid businessId)
        {
            var subscriptions = await _subscriptionRepository
               .GetByBusinessIdAsync(businessId);

            return subscriptions.Adapt<List<SubscriptionDto>>();
        }

        public async Task<List<SubscriptionDto>> GetPendingAsync()
        {
            var subscriptions = await _subscriptionRepository.GetPendingAsync();
            return subscriptions.Adapt<List<SubscriptionDto>>();
        }
      
    }
}
