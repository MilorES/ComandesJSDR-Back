using Microsoft.EntityFrameworkCore;
using ComandesAPI.Models;

namespace ComandesAPI.Data
{
    public static class ClientsSeedData
    {
        /// <summary>
        /// Afegeix clients per defecte a la base de dades
        /// </summary>
        public static void SeedClients(this ModelBuilder modelBuilder)
        {
            var dataCreacio = new DateTime(2025, 1, 15, 10, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    UsuariId = 2, // Usuari estàndard
                    NomEmpresa = "Tecnologies Avançades SL",
                    NIF = "B12345678",
                    Adreca = "Carrer de la Innovació, 42",
                    Poblacio = "Barcelona",
                    Provincia = "Barcelona",
                    CodiPostal = "08028",
                    Pais = "Espanya",
                    Telefon = "+34 932 456 789",
                    Notes = "Client preferent amb descompte del 5% en compres superiors a 1000€",
                    Actiu = true,
                    DataCreacio = dataCreacio
                }
            );
        }
    }
}
