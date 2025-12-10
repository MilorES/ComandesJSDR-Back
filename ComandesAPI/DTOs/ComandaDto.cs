using System.ComponentModel.DataAnnotations;
using ComandesAPI.Models;

namespace ComandesAPI.DTOs;

/// <summary>
/// DTO per retornar informació de la comanda
/// </summary>
public class ComandaDto
{
    public int Id { get; set; }
    public string NumeroComanda { get; set; } = string.Empty;
    public int? UsuariId { get; set; }
    public EstatComanda Estat { get; set; }
    public DateTime DataCreacio { get; set; }
    public DateTime? DataModificacio { get; set; }
    public DateTime? DataAprovacio { get; set; }
    public DateTime? DataFinalitzacio { get; set; }
    public string? Observacions { get; set; }
    public decimal Total { get; set; }
    public decimal DescomptePercentatge { get; set; }
    public decimal ImportDescompte { get; set; }
    public decimal TotalAmbDescompte { get; set; }
    public bool Actiu { get; set; }
    public List<LiniaComandaDto> Linies { get; set; } = new();
}

/// <summary>
/// DTO per crear una nova comanda
/// </summary>
public class CreateComandaDto
{
    [MaxLength(1000, ErrorMessage = "Les observacions no poden superar els 1000 caràcters")]
    public string? Observacions { get; set; }

    [Range(0, 100, ErrorMessage = "El descompte ha d'estar entre 0 i 100")]
    public decimal DescomptePercentatge { get; set; } = 0;

    [Required(ErrorMessage = "Cal especificar almenys una línia de comanda")]
    [MinLength(1, ErrorMessage = "Cal especificar almenys una línia de comanda")]
    public List<CreateLiniaComandaDto> Linies { get; set; } = new();
}

/// <summary>
/// DTO per actualitzar una comanda existent
/// </summary>
public class UpdateComandaDto
{
    [MaxLength(1000, ErrorMessage = "Les observacions no poden superar els 1000 caràcters")]
    public string? Observacions { get; set; }

    [Range(0, 100, ErrorMessage = "El descompte ha d'estar entre 0 i 100")]
    public decimal DescomptePercentatge { get; set; } = 0;

    public List<CreateLiniaComandaDto> Linies { get; set; } = new();
}

/// <summary>
/// DTO per canviar l'estat d'una comanda
/// </summary>
public class CanviarEstatComandaDto
{
    [Required(ErrorMessage = "L'estat és obligatori")]
    public EstatComanda Estat { get; set; }
}
