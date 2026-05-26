using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace LocalMarket.Infrastructure.Security
{
    public class JwtTokenProvider : IJwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly int _expirationMinutes;

        public JwtTokenProvider()
        {
            _key = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                ?? throw new InvalidOperationException("JWT_SECRET_KEY not configured");

            _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                ?? throw new InvalidOperationException("JWT_ISSUER not configured");

            _expirationMinutes = int.TryParse(
                Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES"), out var m) ? m : 15;
        }

        public (string token, string jwtId) GenerateToken(User user)
        {
            var jwtId = Guid.NewGuid().ToString();

            var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: null,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), jwtId);
        }

        public (string rawToken, string tokenHash) GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            var rawToken = Convert.ToBase64String(bytes);
            var tokenHash = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)))
                .ToLowerInvariant();
            return (rawToken, tokenHash);
        }

        public string? ValidateTokenAndGetJwtId(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

                // ValidateLifetime = false porque el access token puede estar expirado
                // cuando se llama al endpoint de refresh — eso es válido
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                };

                handler.ValidateToken(token, parameters, out var validatedToken);
                var jwt = (JwtSecurityToken)validatedToken;

                return jwt.Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;


            }
            catch
            {
                return null;
            }
        }

        public Guid? ValidateTokenAndGetUserId(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                };

                handler.ValidateToken(token, parameters, out var validatedToken);
                var jwt = (JwtSecurityToken)validatedToken;
                var sub = jwt.Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                return sub is null ? null : Guid.Parse(sub);
            }
            catch
            {
                return null;
            }
        }
    }
}
