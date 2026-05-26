using LocalMarket.Core.DTos.Business;
using LocalMarket.Core.Entities;
using Mapster;

namespace LocalMarket.Infrastructure.Mappers
{
    public class BusinessMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // CreateBusinessDto → Business
            config.NewConfig<CreateBusinessDto, Business>()
                .Map(dest => dest.IsActive, _ => true)
                .Map(dest => dest.IsVerified, _ => false)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.UserId)
                .Ignore(dest => dest.LogoUrl!);

            // UpdateBusinessDto → Business
            config.NewConfig<UpdateBusinessDto, Business>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.UserId)
                .Ignore(dest => dest.LogoUrl!)
                .Ignore(dest => dest.IsVerified)
                .Ignore(dest => dest.CreatedAt);
        }
    }
}