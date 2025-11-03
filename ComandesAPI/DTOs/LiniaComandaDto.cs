using System.ComponentModel.DataAnnotations;

namespace ComandesAPI.DTOs;

/// <summary>
/// DTO per retornar informació d'una línia de comanda
/// </summary>
public class LiniaComandaDto
{
    public int Id { get; set; }
    public int ComandaId { get; set; }
    public int? ArticleId { get; set; }
    public string NomProducte { get; set; } = string.Empty;
    public string? Descripcio { get; set; }
    public decimal Quantitat { get; set; }
    public decimal PreuUnitari { get; set; }
    public decimal DescomptePercentatge { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ImportDescompte { get; set; }
    public decimal Total { get; set; }
    public int Ordre { get; set; }
    public DateTime DataCreacio { get; set; }
}

/// <summary>
/// DTO per crear una nova línia de comanda
/// </summary>
public class CreateLiniaComandaDto
{
    public int? ArticleId { get; set; }

    [Required(ErrorMessage = "El nom del producte és obligatori")]
    [MaxLength(200, ErrorMessage = "El nom del producte no pot superar els 200 caràcters")]
    public string NomProducte { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "La descripció no pot superar els 500 caràcters")]
    public string? Descripcio { get; set; }

    [Required(ErrorMessage = "La quantitat és obligatòria")]
    [Range(0.01, double.MaxValue, ErrorMessage = "La quantitat ha de ser superior a 0")]
    public decimal Quantitat { get; set; }

    [Required(ErrorMessage = "El preu unitari és obligatori")]
    [Range(0, double.MaxValue, ErrorMessage = "El preu unitari no pot ser negatiu")]
    public decimal PreuUnitari { get; set; }

    [Range(0, 100, ErrorMessage = "El descompte ha d'estar entre 0 i 100")]
    public decimal DescomptePercentatge { get; set; } = 0;

    public int Ordre { get; set; } = 0;
}
