using Microsoft.EntityFrameworkCore;
using ComandesAPI.Data;
using ComandesAPI.Models;
using BCrypt.Net;

namespace ComandesAPI.Services
{
    /// <summary>
    /// Servei d'autenticació amb hash de contrasenyes BCrypt
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ComandesDbContext _context;

        public AuthService(ComandesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Autentica un usuari verificant el username i la contrasenya
        /// </summary>
        public async Task<Usuari?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Usuaris
                .FirstOrDefaultAsync(u => u.Username == username);

            // Verificar si l'usuari existeix, està habilitat i la contrasenya és correcta
            if (user == null || !user.IsEnabled || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Genera un hash BCrypt per a una contrasenya
        /// </summary>
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        /// <summary>
        /// Verifica si una contrasenya coincideix amb el hash BCrypt
        /// </summary>
        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
