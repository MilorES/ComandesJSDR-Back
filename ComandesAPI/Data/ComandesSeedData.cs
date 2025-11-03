using Microsoft.EntityFrameworkCore;
using ComandesAPI.Models;

namespace ComandesAPI.Data
{
    public static class ComandesSeedData
    {
        /// <summary>
        /// Afegeix comandes i línies de comanda per defecte a la base de dades
        /// </summary>
        public static void SeedComandes(this ModelBuilder modelBuilder)
        {
            var dataBase = new DateTime(2025, 2, 1, 10, 0, 0, DateTimeKind.Utc);

            // ======== COMANDA 1: Esborrany ========
            var comanda1 = new Comanda
            {
                Id = 1,
                NumeroComanda = "COM-2025-000001",
                ClientId = 1,
                Estat = EstatComanda.Esborrany,
                DataCreacio = dataBase,
                Observacions = "Primera comanda en fase d'elaboració",
                Total = 370.48m,
                DescomptePercentatge = 0m,
                ImportDescompte = 0m,
                TotalAmbDescompte = 370.48m,
                Actiu = true
            };

            // Línies comanda 1
            var linies1 = new[]
            {
                new LiniaComanda
                {
                    Id = 1,
                    ComandaId = 1,
                    ArticleId = 2,
                    NomProducte = "Ratolí sense fils",
                    Descripcio = "Ratolí òptic sense fils amb sensor de precisió",
                    Quantitat = 5,
                    PreuUnitari = 25.50m,
                    DescomptePercentatge = 0m,
                    Subtotal = 127.50m,
                    ImportDescompte = 0m,
                    Total = 127.50m,
                    Ordre = 0,
                    DataCreacio = dataBase
                },
                new LiniaComanda
                {
                    Id = 2,
                    ComandaId = 1,
                    ArticleId = 10,
                    NomProducte = "Webcam HD",
                    Descripcio = "Càmera web Full HD 1080p amb micròfon integrat",
                    Quantitat = 3,
                    PreuUnitari = 45.99m,
                    DescomptePercentatge = 0m,
                    Subtotal = 137.97m,
                    ImportDescompte = 0m,
                    Total = 137.97m,
                    Ordre = 1,
                    DataCreacio = dataBase
                },
                new LiniaComanda
                {
                    Id = 3,
                    ComandaId = 1,
                    ArticleId = 6,
                    NomProducte = "Impressora làser",
                    Descripcio = "Impressora làser monocrom amb connexió Wi-Fi",
                    Quantitat = 1,
                    PreuUnitari = 89.99m,
                    DescomptePercentatge = 10m,
                    Subtotal = 89.99m,
                    ImportDescompte = 9.00m,
                    Total = 80.99m,
                    Ordre = 2,
                    DataCreacio = dataBase
                },
                new LiniaComanda
                {
                    Id = 4,
                    ComandaId = 1,
                    ArticleId = 3,
                    NomProducte = "Teclat mecànic",
                    Descripcio = "Teclat mecànic retroil·luminat amb switches Cherry MX",
                    Quantitat = 2,
                    PreuUnitari = 120.00m,
                    DescomptePercentatge = 5m,
                    Subtotal = 240.00m,
                    ImportDescompte = 12.00m,
                    Total = 228.00m,
                    Ordre = 3,
                    DataCreacio = dataBase
                }
            };

            // ======== COMANDA 2: Pendent d'Aprovació ========
            var comanda2 = new Comanda
            {
                Id = 2,
                NumeroComanda = "COM-2025-000002",
                ClientId = 1,
                Estat = EstatComanda.PendentAprovacio,
                DataCreacio = dataBase.AddDays(2),
                Observacions = "Material per nova oficina - urgent",
                Total = 939.96m,
                DescomptePercentatge = 5m,
                ImportDescompte = 47.00m,
                TotalAmbDescompte = 892.96m,
                Actiu = true
            };

            var linies2 = new[]
            {
                new LiniaComanda
                {
                    Id = 5,
                    ComandaId = 2,
                    ArticleId = 4,
                    NomProducte = "Monitor 24 polzades",
                    Descripcio = "Monitor LED Full HD 1920x1080 amb connexió HDMI",
                    Quantitat = 4,
                    PreuUnitari = 189.99m,
                    DescomptePercentatge = 0m,
                    Subtotal = 759.96m,
                    ImportDescompte = 0m,
                    Total = 759.96m,
                    Ordre = 0,
                    DataCreacio = dataBase.AddDays(2)
                },
                new LiniaComanda
                {
                    Id = 6,
                    ComandaId = 2,
                    ArticleId = 7,
                    NomProducte = "Disc dur extern 1TB",
                    Descripcio = "Disc dur extern USB 3.0 de 1TB per còpies de seguretat",
                    Quantitat = 3,
                    PreuUnitari = 59.99m,
                    DescomptePercentatge = 0m,
                    Subtotal = 179.97m,
                    ImportDescompte = 0m,
                    Total = 179.97m,
                    Ordre = 1,
                    DataCreacio = dataBase.AddDays(2)
                }
            };

            // ======== COMANDA 3: Aprovada ========
            var comanda3 = new Comanda
            {
                Id = 3,
                NumeroComanda = "COM-2025-000003",
                ClientId = 1,
                Estat = EstatComanda.Aprovada,
                DataCreacio = dataBase.AddDays(5),
                DataAprovacio = dataBase.AddDays(6),
                Observacions = "Mobiliari per sala de reunions",
                Total = 899.95m,
                DescomptePercentatge = 10m,
                ImportDescompte = 90.00m,
                TotalAmbDescompte = 809.95m,
                Actiu = true
            };

            var linies3 = new[]
            {
                new LiniaComanda
                {
                    Id = 7,
                    ComandaId = 3,
                    ArticleId = 5,
                    NomProducte = "Cadira d'oficina",
                    Descripcio = "Cadira ergonòmica amb suport lumbar ajustable",
                    Quantitat = 4,
                    PreuUnitari = 149.99m,
                    DescomptePercentatge = 0m,
                    Subtotal = 599.96m,
                    ImportDescompte = 0m,
                    Total = 599.96m,
                    Ordre = 0,
                    DataCreacio = dataBase.AddDays(5)
                },
                new LiniaComanda
                {
                    Id = 8,
                    ComandaId = 3,
                    ArticleId = 8,
                    NomProducte = "Taula d'oficina",
                    Descripcio = "Taula d'oficina de fusta amb calaixos i organitzador",
                    Quantitat = 1,
                    PreuUnitari = 299.99m,
                    DescomptePercentatge = 0m,
                    Subtotal = 299.99m,
                    ImportDescompte = 0m,
                    Total = 299.99m,
                    Ordre = 1,
                    DataCreacio = dataBase.AddDays(5)
                }
            };

            // ======== COMANDA 4: En Procés ========
            var comanda4 = new Comanda
            {
                Id = 4,
                NumeroComanda = "COM-2025-000004",
                ClientId = 1,
                Estat = EstatComanda.EnProces,
                DataCreacio = dataBase.AddDays(10),
                DataAprovacio = dataBase.AddDays(11),
                Observacions = "Equipament informàtic complet per 3 treballadors",
                Total = 625.93m,
                DescomptePercentatge = 0m,
                ImportDescompte = 0m,
                TotalAmbDescompte = 625.93m,
                Actiu = true
            };

            var linies4 = new[]
            {
                new LiniaComanda
                {
                    Id = 9,
                    ComandaId = 4,
                    ArticleId = 2,
                    NomProducte = "Ratolí sense fils",
                    Descripcio = "Ratolí òptic sense fils amb sensor de precisió",
                    Quantitat = 3,
                    PreuUnitari = 25.50m,
                    DescomptePercentatge = 0m,
                    Subtotal = 76.50m,
                    ImportDescompte = 0m,
                    Total = 76.50m,
                    Ordre = 0,
                    DataCreacio = dataBase.AddDays(10)
                },
                new LiniaComanda
                {
                    Id = 10,
                    ComandaId = 4,
                    ArticleId = 3,
                    NomProducte = "Teclat mecànic",
                    Descripcio = "Teclat mecànic retroil·luminat amb switches Cherry MX",
                    Quantitat = 3,
                    PreuUnitari = 120.00m,
                    DescomptePercentatge = 0m,
                    Subtotal = 360.00m,
                    ImportDescompte = 0m,
                    Total = 360.00m,
                    Ordre = 1,
                    DataCreacio = dataBase.AddDays(10)
                },
                new LiniaComanda
                {
                    Id = 11,
                    ComandaId = 4,
                    ArticleId = 9,
                    NomProducte = "Auriculars Bluetooth",
                    Descripcio = "Auriculars inalàmbrics amb cancel·lació de soroll",
                    Quantitat = 3,
                    PreuUnitari = 79.99m,
                    DescomptePercentatge = 5m,
                    Subtotal = 239.97m,
                    ImportDescompte = 12.00m,
                    Total = 227.97m,
                    Ordre = 2,
                    DataCreacio = dataBase.AddDays(10)
                }
            };

            // ======== COMANDA 5: Finalitzada ========
            var comanda5 = new Comanda
            {
                Id = 5,
                NumeroComanda = "COM-2025-000005",
                ClientId = 1,
                Estat = EstatComanda.Finalitzada,
                DataCreacio = dataBase.AddDays(15),
                DataAprovacio = dataBase.AddDays(16),
                DataFinalitzacio = dataBase.AddDays(20),
                Observacions = "Comanda completada i lliurada. Client satisfet.",
                Total = 1099.94m,
                DescomptePercentatge = 5m,
                ImportDescompte = 55.00m,
                TotalAmbDescompte = 1044.94m,
                Actiu = true
            };

            var linies5 = new[]
            {
                new LiniaComanda
                {
                    Id = 12,
                    ComandaId = 5,
                    ArticleId = 4,
                    NomProducte = "Monitor 24 polzades",
                    Descripcio = "Monitor LED Full HD 1920x1080 amb connexió HDMI",
                    Quantitat = 2,
                    PreuUnitari = 189.99m,
                    DescomptePercentatge = 0m,
                    Subtotal = 379.98m,
                    ImportDescompte = 0m,
                    Total = 379.98m,
                    Ordre = 0,
                    DataCreacio = dataBase.AddDays(15)
                },
                new LiniaComanda
                {
                    Id = 13,
                    ComandaId = 5,
                    ArticleId = 8,
                    NomProducte = "Taula d'oficina",
                    Descripcio = "Taula d'oficina de fusta amb calaixos i organitzador",
                    Quantitat = 2,
                    PreuUnitari = 299.99m,
                    DescomptePercentatge = 0m,
                    Subtotal = 599.98m,
                    ImportDescompte = 0m,
                    Total = 599.98m,
                    Ordre = 1,
                    DataCreacio = dataBase.AddDays(15)
                },
                new LiniaComanda
                {
                    Id = 14,
                    ComandaId = 5,
                    ArticleId = 7,
                    NomProducte = "Disc dur extern 1TB",
                    Descripcio = "Disc dur extern USB 3.0 de 1TB per còpies de seguretat",
                    Quantitat = 2,
                    PreuUnitari = 59.99m,
                    DescomptePercentatge = 0m,
                    Subtotal = 119.98m,
                    ImportDescompte = 0m,
                    Total = 119.98m,
                    Ordre = 2,
                    DataCreacio = dataBase.AddDays(15)
                }
            };

            // Afegir totes les dades
            modelBuilder.Entity<Comanda>().HasData(comanda1, comanda2, comanda3, comanda4, comanda5);
            modelBuilder.Entity<LiniaComanda>().HasData(linies1);
            modelBuilder.Entity<LiniaComanda>().HasData(linies2);
            modelBuilder.Entity<LiniaComanda>().HasData(linies3);
            modelBuilder.Entity<LiniaComanda>().HasData(linies4);
            modelBuilder.Entity<LiniaComanda>().HasData(linies5);
        }
    }
}
