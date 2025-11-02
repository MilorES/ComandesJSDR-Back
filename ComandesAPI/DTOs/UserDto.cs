using System.ComponentModel.DataAnnotations;

namespace ComandesAPI.DTOs
{
    /// <summary>
    /// DTO per a la informació d'usuari (sense contrasenya)
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsEnabled { get; set; }
    }

    /// <summary>
    /// DTO per a crear un nou usuari
    /// </summary>
    public class CreateUserDto
    {
        [Required(ErrorMessage = "El nom d'usuari és obligatori")]
        [MaxLength(50, ErrorMessage = "El nom d'usuari no pot superar els 50 caràcters")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contrasenya és obligatòria")]
        [MinLength(6, ErrorMessage = "La contrasenya ha de tenir almenys 6 caràcters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nom complet és obligatori")]
        [MaxLength(100, ErrorMessage = "El nom complet no pot superar els 100 caràcters")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email és obligatori")]
        [EmailAddress(ErrorMessage = "L'email no és vàlid")]
        [MaxLength(100, ErrorMessage = "L'email no pot superar els 100 caràcters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol és obligatori")]
        [RegularExpression("^(User|Administrator)$", ErrorMessage = "El rol ha de ser 'User' o 'Administrator'")]
        public string Role { get; set; } = "User";

        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// DTO per a actualitzar un usuari existent
    /// </summary>
    public class UpdateUserDto
    {
        [MaxLength(100, ErrorMessage = "El nom complet no pot superar els 100 caràcters")]
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "L'email no és vàlid")]
        [MaxLength(100, ErrorMessage = "L'email no pot superar els 100 caràcters")]
        public string? Email { get; set; }

        [RegularExpression("^(User|Administrator)$", ErrorMessage = "El rol ha de ser 'User' o 'Administrator'")]
        public string? Role { get; set; }

        public bool? IsEnabled { get; set; }
    }

    /// <summary>
    /// DTO per a canviar la contrasenya
    /// </summary>
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "La contrasenya actual és obligatòria")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nova contrasenya és obligatòria")]
        [MinLength(6, ErrorMessage = "La nova contrasenya ha de tenir almenys 6 caràcters")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
