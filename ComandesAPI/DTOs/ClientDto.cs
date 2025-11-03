using System.ComponentModel.DataAnnotations;

namespace ComandesAPI.DTOs;

/// <summary>
/// DTO per retornar informació del client
/// </summary>
public class ClientDto
{
    public int Id { get; set; }
    public int UsuariId { get; set; }
    public string NomEmpresa { get; set; } = string.Empty;
    public string NIF { get; set; } = string.Empty;
    public string? Adreca { get; set; }
    public string? Poblacio { get; set; }
    public string? Provincia { get; set; }
    public string? CodiPostal { get; set; }
    public string? Pais { get; set; }
    public string? Telefon { get; set; }
    public string? Notes { get; set; }
    public bool Actiu { get; set; }
    public DateTime DataCreacio { get; set; }
    public DateTime? DataModificacio { get; set; }
}

/// <summary>
/// DTO per crear un nou client
/// </summary>
public class CreateClientDto
{
    [Required(ErrorMessage = "El nom de l'empresa és obligatori")]
    [MaxLength(100, ErrorMessage = "El nom de l'empresa no pot superar els 100 caràcters")]
    public string NomEmpresa { get; set; } = string.Empty;

    [Required(ErrorMessage = "El NIF és obligatori")]
    [MaxLength(20, ErrorMessage = "El NIF no pot superar els 20 caràcters")]
    public string NIF { get; set; } = string.Empty;

    [MaxLength(200, ErrorMessage = "L'adreça no pot superar els 200 caràcters")]
    public string? Adreca { get; set; }

    [MaxLength(100, ErrorMessage = "La població no pot superar els 100 caràcters")]
    public string? Poblacio { get; set; }

    [MaxLength(50, ErrorMessage = "La província no pot superar els 50 caràcters")]
    public string? Provincia { get; set; }

    [MaxLength(10, ErrorMessage = "El codi postal no pot superar els 10 caràcters")]
    public string? CodiPostal { get; set; }

    [MaxLength(50, ErrorMessage = "El país no pot superar els 50 caràcters")]
    public string? Pais { get; set; }

    [Phone(ErrorMessage = "El format del telèfon no és vàlid")]
    [MaxLength(20, ErrorMessage = "El telèfon no pot superar els 20 caràcters")]
    public string? Telefon { get; set; }

    [MaxLength(500, ErrorMessage = "Les notes no poden superar els 500 caràcters")]
    public string? Notes { get; set; }
}

/// <summary>
/// DTO per actualitzar un client existent
/// </summary>
public class UpdateClientDto
{
    [Required(ErrorMessage = "El nom de l'empresa és obligatori")]
    [MaxLength(100, ErrorMessage = "El nom de l'empresa no pot superar els 100 caràcters")]
    public string NomEmpresa { get; set; } = string.Empty;

    [Required(ErrorMessage = "El NIF és obligatori")]
    [MaxLength(20, ErrorMessage = "El NIF no pot superar els 20 caràcters")]
    public string NIF { get; set; } = string.Empty;

    [MaxLength(200, ErrorMessage = "L'adreça no pot superar els 200 caràcters")]
    public string? Adreca { get; set; }

    [MaxLength(100, ErrorMessage = "La població no pot superar els 100 caràcters")]
    public string? Poblacio { get; set; }

    [MaxLength(50, ErrorMessage = "La província no pot superar els 50 caràcters")]
    public string? Provincia { get; set; }

    [MaxLength(10, ErrorMessage = "El codi postal no pot superar els 10 caràcters")]
    public string? CodiPostal { get; set; }

    [MaxLength(50, ErrorMessage = "El país no pot superar els 50 caràcters")]
    public string? Pais { get; set; }

    [Phone(ErrorMessage = "El format del telèfon no és vàlid")]
    [MaxLength(20, ErrorMessage = "El telèfon no pot superar els 20 caràcters")]
    public string? Telefon { get; set; }

    [MaxLength(500, ErrorMessage = "Les notes no poden superar els 500 caràcters")]
    public string? Notes { get; set; }

    public bool Actiu { get; set; } = true;
}
