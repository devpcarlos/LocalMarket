
using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
