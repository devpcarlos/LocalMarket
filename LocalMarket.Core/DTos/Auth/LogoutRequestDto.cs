
using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Auth
{
    public class LogoutRequestDto
    {

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
