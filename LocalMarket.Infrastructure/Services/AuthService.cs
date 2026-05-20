using Mapster;
using LocalMarket.Core.DTos.Auth;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence.Models;
using MapsterMapper;
namespace LocalMarket.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly Supabase.Client _supabase;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly int _expirationHours;
        private readonly IMapper _mapper;

        public AuthService(
      Supabase.Client supabase,
      IUserRepository userRepository,
      IJwtService jwtService, IMapper mapper)
        {
            _supabase = supabase;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _expirationHours = int.TryParse(
                Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS"), out var h) ? h : 1;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {

            /** ── MOCK TEMPORAL — quitar cuando Supabase esté listo ──────
            var mockUser = new Core.Entities.User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = request.Email.ToLowerInvariant().Trim(),
                Phone = request.Phone?.Trim(),
                CreatedAt = DateTime.UtcNow
            };
            return BuildAuthResponse(mockUser);
            // ── FIN MOCK ──── **/

            var session = await _supabase.Auth.SignUp(request.Email, request.Password);

            if (session?.User?.Id == null)
                throw new InvalidOperationException("Registration failed");

            // 3. Establecer sesión para que RLS permita el INSERT
            if (session.AccessToken != null)
                await _supabase.Auth.SetSession(
                    session.AccessToken, session.RefreshToken!);

            // USAMOS EL MAPPER INYECTADO
            var model = _mapper.Map<UserModel>(request);
            model.Id = Guid.Parse(session.User.Id);

            var entity = _mapper.Map<Core.Entities.User>(model);
            await _userRepository.CreateAsync(entity);

            return BuildAuthResponse(entity); 
        }
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {

            /** ── MOCK TEMPORAL ───────────────────────────────────────────
            var mockUser = new Core.Entities.User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Mock User",
                Email = request.Email.ToLowerInvariant().Trim(),
                Phone = null,
                Role = "client",
                CreatedAt = DateTime.UtcNow
            };
            return BuildAuthResponse(mockUser);
            ── FIN MOCK ─────**/
            var session = await _supabase.Auth.SignIn(request.Email, request.Password);

              if (session?.User?.Id == null)
                  throw new UnauthorizedAccessException("Invalid credentials");

              var user = await _userRepository.GetByIdAsync(Guid.Parse(session.User.Id));

              if (user is null)
                  throw new UnauthorizedAccessException("Invalid credentials");

              return BuildAuthResponse(user); 
        }

        public async Task RecoverPasswordAsync(string email)
        {
            await _supabase.Auth.ResetPasswordForEmail(email);
        }
        private AuthResponseDto BuildAuthResponse(Core.Entities.User user)
        {
            var response = user.Adapt<AuthResponseDto>();
            response.Token = _jwtService.GenerateToken(user);
            response.ExpiresAt = DateTime.UtcNow.AddHours(_expirationHours);

            return response;
        }
    }
}
