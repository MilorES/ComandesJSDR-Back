using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComandesAPI.Models;

/// <summary>
/// Comanda d'un client
/// </summary>
public class Comanda
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Número de comanda únic
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string NumeroComanda { get; set; } = string.Empty;

    /// <summary>
    /// FK al client que fa la comanda
    /// </summary>
    [Required]
    public int ClientId { get; set; }

    /// <summary>
    /// Propietat de navegació al Client
    /// </summary>
    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; } = null!;

    /// <summary>
    /// Estat actual de la comanda
    /// </summary>
    [Required]
    public EstatComanda Estat { get; set; } = EstatComanda.Esborrany;

    /// <summary>
    /// Data de creació de la comanda
    /// </summary>
    public DateTime DataCreacio { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data de l'última modificació
    /// </summary>
    public DateTime? DataModificacio { get; set; }

    /// <summary>
    /// Data d'aprovació de la comanda
    /// </summary>
    public DateTime? DataAprovacio { get; set; }

    /// <summary>
    /// Data de finalització de la comanda
    /// </summary>
    public DateTime? DataFinalitzacio { get; set; }

    /// <summary>
    /// Observacions generals de la comanda
    /// </summary>
    [MaxLength(1000)]
    public string? Observacions { get; set; }

    /// <summary>
    /// Import total de la comanda (calculat)
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; } = 0;

    /// <summary>
    /// Descompte aplicat (percentatge)
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100)]
    public decimal DescomptePercentatge { get; set; } = 0;

    /// <summary>
    /// Import del descompte (calculat)
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal ImportDescompte { get; set; } = 0;

    /// <summary>
    /// Import total amb descompte aplicat
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmbDescompte { get; set; } = 0;

    public bool Actiu { get; set; } = true;

    /// <summary>
    /// Col·lecció de línies de la comanda
    /// </summary>
    public ICollection<LiniaComanda> Linies { get; set; } = new List<LiniaComanda>();
}
