using Mapster;
using LocalMarket.Core.DTos.Auth;
using LocalMarket.Core.Interfaces;
using LocalMarket.Core.Entities;

namespace LocalMarket.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly int _expirationHours;

        public AuthService(
      IUserRepository userRepository,
      IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _expirationHours = int.TryParse(
                Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS"), out var h) ? h : 1;
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

            return BuildAuthResponse(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email) ??
                throw new UnauthorizedAccessException("Invalid credentials");
            
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            return BuildAuthResponse(user);
        }

        public async Task RecoverPasswordAsync(string email)
        {
            await Task.CompletedTask;
        }
        private AuthResponseDto BuildAuthResponse(User user)
        {
            var response = user.Adapt<AuthResponseDto>();
            response.Token = _jwtService.GenerateToken(user);
            response.RefreshToken = _jwtService.GenerateRefreshToken();
            response.ExpiresAt = DateTime.UtcNow.AddHours(_expirationHours);

            return response;
        }
    }
}
