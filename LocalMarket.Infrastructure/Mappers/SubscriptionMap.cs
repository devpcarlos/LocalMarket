

using LocalMarket.Core.DTos.Subscriptions;
using LocalMarket.Core.Entities;
using LocalMarket.Core.Enums;
using Mapster;

namespace LocalMarket.Infrastructure.Mappers
{
    public class SubscriptionMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Subscription, SubscriptionDto>()
                .Map(dest => dest.Plan, src => src.Plan.ToString());

            config.NewConfig<CreateSubscriptionDto, Subscription>()
                .Map(dest => dest.Plan,
                    src => Enum.Parse<SubscriptionPlan>(src.Plan, ignoreCase: true))
                .Map(dest => dest.IsActive, _ => false)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.BusinessId)
                .Ignore(dest => dest.MonthlyPrice)
                .Ignore(dest => dest.StartDate)
                .Ignore(dest => dest.EndDate)
                .Ignore(dest => dest.ActivatedBy!)
                .Ignore(dest => dest.ActivatedAt!);
        }
    }
}
