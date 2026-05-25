using LocalMarket.Core.DTos.Auth;

namespace LocalMarket.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, string ipAddress);
        Task LogoutAsync(string refreshToken);
        Task RecoverPasswordAsync(string email);
    }
}
