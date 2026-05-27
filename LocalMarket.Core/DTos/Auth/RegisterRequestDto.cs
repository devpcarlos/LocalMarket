using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Auth
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string Email { get; set; } = string.Empty;

        [RegularExpression("^(Client|BusinessOwner)$",
        ErrorMessage = "Role must be 'Client' or 'BusinessOwner'")]
        public string Role { get; set; } = "Client";

        [Phone(ErrorMessage = "Teléfono no válido")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "Mínimo 8 caracteres")]
        public string Password { get; set; } = string.Empty;
    }
}
