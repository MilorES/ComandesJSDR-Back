using System.ComponentModel.DataAnnotations;

namespace ComandesAPI.DTOs
{
    /// <summary>
    /// DTO per a la petició de login
    /// </summary>
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El nom d'usuari és obligatori")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contrasenya és obligatòria")]
        public string Password { get; set; } = string.Empty;
    }
}
