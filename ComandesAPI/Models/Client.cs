using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComandesAPI.Models;

/// <summary>
/// Client relacionat amb un usuari (relació 1-1)
/// </summary>
public class Client
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// FK a la taula Usuaris (relació 1-1)
    /// </summary>
    [Required]
    public int UsuariId { get; set; }

    /// <summary>
    /// Propietat de navegació a Usuari
    /// </summary>
    [ForeignKey(nameof(UsuariId))]
    public Usuari Usuari { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string NomEmpresa { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string NIF { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Adreca { get; set; }

    [MaxLength(100)]
    public string? Poblacio { get; set; }

    [MaxLength(50)]
    public string? Provincia { get; set; }

    [MaxLength(10)]
    public string? CodiPostal { get; set; }

    [MaxLength(50)]
    public string? Pais { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? Telefon { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public bool Actiu { get; set; } = true;

    public DateTime DataCreacio { get; set; } = DateTime.UtcNow;

    public DateTime? DataModificacio { get; set; }

    /// <summary>
    /// Col·lecció de comandes del client
    /// </summary>
    public ICollection<Comanda> Comandes { get; set; } = new List<Comanda>();
}
