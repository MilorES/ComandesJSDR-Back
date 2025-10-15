using Microsoft.EntityFrameworkCore;
using ComandesAPI.Models;

namespace ComandesAPI.Data
{
    public static class ArticlesSeedData
    {
        public static void SeedArticles(this ModelBuilder modelBuilder)
        {
            var dataCreacio = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Article>().HasData(
                new Article
                {
                    Id = 1,
                    Nom = "Ordinador portàtil",
                    Descripcio = "Ordinador portàtil per oficina amb pantalla de 15.6 polzades",
                    Preu = 899.99m,
                    Estoc = 10,
                    Categoria = "Informàtica",
                    Actiu = true,
                    DataCreacio = dataCreacio
                },
                new Article
                {
                    Id = 2,
                    Nom = "Ratolí sense fils",
                    Descripcio = "Ratolí òptic sense fils amb sensor de precisió",
                    Preu = 25.50m,
                    Estoc = 50,
                    Categoria = "Informàtica",
                    Actiu = true,
                    DataCreacio = dataCreacio
                },
                new Article
                {
                    Id = 3,
                    Nom = "Teclat mecànic",
                    Descripcio = "Teclat mecànic retroil·luminat amb switches Cherry MX",
                    Preu = 120.00m,
                    Estoc = 25,
                    Categoria = "Informàtica",
                    Actiu = true,
                    DataCreacio = dataCreacio
                },
                new Article
                {
                    Id = 4,
                    Nom = "Monitor 24 polzades",
                    Descripcio = "Monitor LED Full HD 1920x1080 amb connexió HDMI",
                    Preu = 189.99m,
                    Estoc = 15,
                    Categoria = "Informàtica",
                    Actiu = true,
                    DataCreacio = dataCreacio
                },
                new Article
                {
                    Id = 5,
                    Nom = "Cadira d'oficina",
                    Descripcio = "Cadira ergonòmica amb suport lumbar ajustable",
                    Preu = 149.99m,
                    Estoc = 8,
                    Categoria = "Mobiliari",
                    Actiu = true,
                    DataCreacio = dataCreacio
                },
                new Article
                {
                    Id = 6,
                    Nom = "Impressora làser",
                    Descripcio = "Impressora làser monocrom amb connexió Wi-Fi",
                    Preu = 89.99m,
                    Estoc = 12,
                    Categoria = "Informàtica",
                    Actiu = true,
                    DataCreacio = dataCreacio
                },
                new Article
                {
                    Id = 7,
                    Nom = "Disc dur extern 1TB",
                    Descripcio = "Disc dur extern USB 3.0 de 1TB per còpies de seguretat",
                    Preu = 59.99m,
                    Estoc = 30,
                    Categoria = "Informàtica",
                    Actiu = true,
                    DataCreacio = dataCreacio
                },
                new Article
                {
                    Id = 8,
                    Nom = "Taula d'oficina",
                    Descripcio = "Taula d'oficina de fusta amb calaixos i organitzador",
                    Preu = 299.99m,
                    Estoc = 5,
                    Categoria = "Mobiliari",
                    Actiu = true,
                    DataCreacio = dataCreacio
                },
                new Article
                {
                    Id = 9,
                    Nom = "Auriculars Bluetooth",
                    Descripcio = "Auriculars inalàmbrics amb cancel·lació de soroll",
                    Preu = 79.99m,
                    Estoc = 20,
                    Categoria = "Àudio",
                    Actiu = true,
                    DataCreacio = dataCreacio
                },
                new Article
                {
                    Id = 10,
                    Nom = "Webcam HD",
                    Descripcio = "Càmera web Full HD 1080p amb micròfon integrat",
                    Preu = 45.99m,
                    Estoc = 18,
                    Categoria = "Informàtica",
                    Actiu = true,
                    DataCreacio = dataCreacio
                }
            );
        }
    }
}
