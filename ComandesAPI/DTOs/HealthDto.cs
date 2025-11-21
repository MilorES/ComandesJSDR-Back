namespace ComandesAPI.DTOs;

/// <summary>
/// Informació de l'estat del servei
/// </summary>
public class HealthDto
{
    /// <summary>
    /// Estat actual del servei
    /// </summary>
    /// <example>Servei actiu</example>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Marca temporal de quan s'ha generat la resposta (UTC)
    /// </summary>
    /// <example>2025-11-21T14:30:00Z</example>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Nom del servei
    /// </summary>
    /// <example>API de Comandes JDSR</example>
    public string Service { get; set; } = string.Empty;

    /// <summary>
    /// Versió de l'API (format: Major.Minor.Patch)
    /// </summary>
    /// <example>0.5.0</example>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Data i hora de compilació (format: YYYYMMDDHHMM)
    /// </summary>
    /// <example>202511211430</example>
    public string Build { get; set; } = string.Empty;
}
