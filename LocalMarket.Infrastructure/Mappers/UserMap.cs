using Mapster;
using LocalMarket.Core.DTos.Auth;
using LocalMarket.Core.Entities;
using LocalMarket.Core.Enums;
namespace LocalMarket.Infrastructure.Mappers
{
    public class UserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // RegisterRequestDto → UserModel
            config.NewConfig<RegisterRequestDto, User>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.PasswordHash)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Map(dest => dest.Email, src => src.Email.ToLowerInvariant().Trim())
                .Map(dest => dest.Name, src => src.Name.Trim())
                .Map(dest => dest.Role, src => Enum.Parse<UserRole>(src.Role, ignoreCase: true))
                .Map(dest => dest.Phone, src => src.Phone == null ? null : src.Phone.Trim());

            // User → AuthResponseDto
            config.NewConfig<User, AuthResponseDto>()
                .Map( dest => dest.Role, src => src.Role )
                .Ignore(dest => dest.Token)
                .Ignore(dest => dest.RefreshToken)
                .Ignore(dest => dest.ExpiresAt);
        }
    }
}