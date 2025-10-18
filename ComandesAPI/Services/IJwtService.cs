using ComandesAPI.Models;

namespace ComandesAPI.Services
{
    /// <summary>
    /// Interfície per al servei de gestió de tokens JWT
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Genera un token JWT per a un usuari
        /// </summary>
        string GenerateToken(Usuari user);

        /// <summary>
        /// Valida un token JWT i retorna l'ID de l'usuari
        /// </summary>
        int? ValidateToken(string token);
    }
}
