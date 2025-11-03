using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComandesAPI.Models;

/// <summary>
/// Línia individual d'una comanda (detall de producte/article)
/// </summary>
public class LiniaComanda
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// FK a la comanda
    /// </summary>
    [Required]
    public int ComandaId { get; set; }

    /// <summary>
    /// Propietat de navegació a Comanda
    /// </summary>
    [ForeignKey(nameof(ComandaId))]
    public Comanda Comanda { get; set; } = null!;

    /// <summary>
    /// FK a l'article (opcional si es vol poder esborrar articles)
    /// </summary>
    public int? ArticleId { get; set; }

    /// <summary>
    /// Propietat de navegació a Article
    /// </summary>
    [ForeignKey(nameof(ArticleId))]
    public Article? Article { get; set; }

    /// <summary>
    /// Nom del producte/servei (guardat per si s'esborra l'article)
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string NomProducte { get; set; } = string.Empty;

    /// <summary>
    /// Descripció del producte
    /// </summary>
    [MaxLength(500)]
    public string? Descripcio { get; set; }

    /// <summary>
    /// Quantitat del producte
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue)]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Quantitat { get; set; }

    /// <summary>
    /// Preu unitari del producte
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal PreuUnitari { get; set; }

    /// <summary>
    /// Descompte aplicat a la línia (percentatge)
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100)]
    public decimal DescomptePercentatge { get; set; } = 0;

    /// <summary>
    /// Import total de la línia (Quantitat * PreuUnitari)
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Import del descompte (calculat)
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal ImportDescompte { get; set; } = 0;

    /// <summary>
    /// Import total amb descompte (Subtotal - ImportDescompte)
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }

    /// <summary>
    /// Ordre de la línia dins la comanda
    /// </summary>
    public int Ordre { get; set; } = 0;

    public DateTime DataCreacio { get; set; } = DateTime.UtcNow;
}
