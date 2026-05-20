using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Auth
{
    public class RecoverPasswordRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
    }
}
