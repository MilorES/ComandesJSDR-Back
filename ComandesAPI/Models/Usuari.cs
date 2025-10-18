using System.ComponentModel.DataAnnotations;

namespace ComandesAPI.Models
{
    /// <summary>
    /// Model que representa un usuari del sistema
    /// </summary>
    public class Usuari
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User"; // "User" o "Administrator"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsEnabled { get; set; } = true;
    }
}
