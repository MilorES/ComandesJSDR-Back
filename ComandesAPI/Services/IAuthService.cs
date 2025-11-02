using ComandesAPI.Models;

namespace ComandesAPI.Services
{
    /// <summary>
    /// Interfície per al servei d'autenticació
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Autentica un usuari amb username i contrasenya
        /// </summary>
        Task<Usuari?> AuthenticateAsync(string username, string password);

        /// <summary>
        /// Genera el hash d'una contrasenya
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Verifica si una contrasenya coincideix amb el hash
        /// </summary>
        bool VerifyPassword(string password, string passwordHash);
    }
}
