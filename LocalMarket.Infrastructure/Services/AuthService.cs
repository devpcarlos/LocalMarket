using Mapster;
using LocalMarket.Core.DTos.Auth;
using LocalMarket.Core.Interfaces;
using LocalMarket.Core.Entities;
using System.Security.Cryptography;
using System.Text;

namespace LocalMarket.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly int _accessExpirationMinutes;
        private readonly int _refreshExpirationDays;

        public AuthService(
      IUserRepository userRepository,
      IRefreshTokenRepository refreshTokenRepository,
      IJwtService jwtService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
            _accessExpirationMinutes = int.TryParse(
                Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES"), out var m) ? m : 15;
            _refreshExpirationDays = int.TryParse(
                Environment.GetEnvironmentVariable("REFRESH_TOKEN_EXPIRATION_DAYS"), out var d) ? d : 7;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var presentUser = await _userRepository.GetByEmailAsync(request.Email);
            if (presentUser is not null)
                throw new InvalidOperationException("Email already in use");

            var user = request.Adapt<User>();
            user.Id = Guid.NewGuid();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _userRepository.CreateAsync(user);

            return await BuildAuthResponse(user, ipAddress: null);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email) ??
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            return await BuildAuthResponse(user, ipAddress: null);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, string ipAddress)
        {
            // 1. Validar firma del access token y extraer jti
            var jwtId = _jwtService.ValidateTokenAndGetJwtId(request.AccessToken);
            if (jwtId is null)
                throw new UnauthorizedAccessException("Invalid access token");

            // 2. Hashear el refresh token recibido y buscarlo en DB
            var tokenHash = HashToken(request.RefreshToken);
            var storedToken = await _refreshTokenRepository.GetByHashAsync(tokenHash);

            // 3. Si no existe → posible reuso malicioso → revocar toda la familia
            if (storedToken is null)
            {
                var userId = _jwtService.ValidateTokenAndGetUserId(request.AccessToken);
                if (userId.HasValue)
                    await _refreshTokenRepository.RevokeAllByUserAsync(userId.Value);
                throw new UnauthorizedAccessException("Invalid refresh token");
            }
            // 4. Verificar que el refresh token pertenece a este access token (jti)
            if (storedToken.JwtId != jwtId)
                throw new UnauthorizedAccessException("Token mismatch");

            // 5. Verificar que no expiró
            if (!storedToken.IsActive)
            {
                await _refreshTokenRepository.RevokeAsync(storedToken);
                throw new UnauthorizedAccessException("Refresh token expired or revoked");
            }

            // 6. Rotación — invalidar el token actual
            await _refreshTokenRepository.RevokeAsync(storedToken);

            // 7. Obtener usuario y generar nuevos tokens
            var user = await _userRepository.GetByIdAsync(storedToken.UserId)
                ?? throw new UnauthorizedAccessException("User not found");
            return await BuildAuthResponse(user, ipAddress);
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var tokenHash = HashToken(refreshToken);
            var storedToken = await _refreshTokenRepository.GetByHashAsync(tokenHash);
            if (storedToken is not null)
                await _refreshTokenRepository.RevokeAsync(storedToken);
        }

        public async Task RecoverPasswordAsync(string email)
        {
            await Task.CompletedTask;
        }
        private async Task<AuthResponseDto> BuildAuthResponse(User user, string? ipAddress)
        {
            var (accessToken, jwtId) = _jwtService.GenerateToken(user);
            var (rawRefreshToken, tokenHash) = _jwtService.GenerateRefreshToken();

            await _refreshTokenRepository.SaveAsync(new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = tokenHash,
                JwtId = jwtId,
                ExpiresAt = DateTime.UtcNow.AddDays(_refreshExpirationDays),
                CreatedAt = DateTime.UtcNow,
                IpAddress = ipAddress,
                IsRevoked = false
            });

            var response = user.Adapt<AuthResponseDto>();
            response.Token = accessToken;
            response.RefreshToken = rawRefreshToken;
            response.ExpiresAt = DateTime.UtcNow.AddMinutes(_accessExpirationMinutes);

            return response;
        }

        private static string HashToken(string rawToken) =>
            Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(rawToken))
            ).ToLowerInvariant();
       
    }
}