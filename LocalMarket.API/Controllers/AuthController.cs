using LocalMarket.Core.DTos.Auth;
using LocalMarket.Core.DTos.Common;
using LocalMarket.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalMarket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            var response = ApiResponseDto<AuthResponseDto>.Ok(result, "Registration successful");
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(ApiResponseDto<AuthResponseDto>.Ok(result, "Login successful"));
        }

        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword(
            [FromBody] RecoverPasswordRequestDto request)
        {
            await _authService.RecoverPasswordAsync(request.Email);
            return Ok(ApiResponseDto<string?>.Ok(null,
                "If this email exists, a recovery link has been sent"));
        }
    }
}
