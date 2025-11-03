namespace ComandesAPI.Models;

/// <summary>
/// Estats possibles d'una comanda
/// </summary>
public enum EstatComanda
{
    /// <summary>
    /// Comanda creada però encara en esborrany
    /// </summary>
    Esborrany = 0,

    /// <summary>
    /// Comanda pendent d'aprovació
    /// </summary>
    PendentAprovacio = 1,

    /// <summary>
    /// Comanda aprovada i confirmada
    /// </summary>
    Aprovada = 2,

    /// <summary>
    /// Comanda en procés de preparació
    /// </summary>
    EnProces = 3,

    /// <summary>
    /// Comanda enviada al client
    /// </summary>
    Enviada = 4,

    /// <summary>
    /// Comanda finalitzada i completada
    /// </summary>
    Finalitzada = 5,

    /// <summary>
    /// Comanda cancel·lada
    /// </summary>
    Cancellada = 6
}
