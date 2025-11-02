using Microsoft.EntityFrameworkCore;
using ComandesAPI.Models;

namespace ComandesAPI.Data
{
    public static class UsersSeedData
    {
        /// <summary>
        /// Afegeix usuaris per defecte a la base de dades
        /// </summary>
        public static void SeedUsers(this ModelBuilder modelBuilder)
        {
            var dataCreacio = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Hash BCrypt de la contrasenya "ComandesJSDR"
            // Generat amb: BCrypt.Net.BCrypt.HashPassword("ComandesJSDR", BCrypt.Net.BCrypt.GenerateSalt(12))
            // IMPORTANT: Aquest hash és estàtic per evitar que EF Core detecti canvis al model cada vegada
            var passwordHash = "$2a$12$wKQgs3QYMJdHm791BDWZ7eJCndZsZAvQYcbBQ9UCEs.sFP6Hp1LOW";

            modelBuilder.Entity<Usuari>().HasData(
                // Usuari administrador
                new Usuari
                {
                    Id = 1,
                    Username = "administrador",
                    PasswordHash = passwordHash,
                    FullName = "Administrador del Sistema",
                    Email = "admin@comandesjdsr.com",
                    Role = "Administrator",
                    IsEnabled = true,
                    CreatedAt = dataCreacio
                },
                // Usuari estàndard
                new Usuari
                {
                    Id = 2,
                    Username = "usuari",
                    PasswordHash = passwordHash,
                    FullName = "Usuari Estàndard",
                    Email = "usuari@comandesjdsr.com",
                    Role = "User",
                    IsEnabled = true,
                    CreatedAt = dataCreacio
                }
            );
        }
    }
}
