using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComandesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AfegirSeedDataClientsComandesLinies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Actiu", "Adreca", "CodiPostal", "DataCreacio", "DataModificacio", "NIF", "NomEmpresa", "Notes", "Pais", "Poblacio", "Provincia", "Telefon", "UsuariId" },
                values: new object[] { 1, true, "Carrer de la Innovació, 42", "08028", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), null, "B12345678", "Tecnologies Avançades SL", "Client preferent amb descompte del 5% en compres superiors a 1000€", "Espanya", "Barcelona", "Barcelona", "+34 932 456 789", 2 });

            migrationBuilder.InsertData(
                table: "Comandes",
                columns: new[] { "Id", "Actiu", "ClientId", "DataAprovacio", "DataCreacio", "DataFinalitzacio", "DataModificacio", "DescomptePercentatge", "Estat", "ImportDescompte", "NumeroComanda", "Observacions", "Total", "TotalAmbDescompte" },
                values: new object[,]
                {
                    { 1, true, 1, null, new DateTime(2025, 2, 1, 10, 0, 0, 0, DateTimeKind.Utc), null, null, 0m, 0, 0m, "COM-2025-000001", "Primera comanda en fase d'elaboració", 370.48m, 370.48m },
                    { 2, true, 1, null, new DateTime(2025, 2, 3, 10, 0, 0, 0, DateTimeKind.Utc), null, null, 5m, 1, 47.00m, "COM-2025-000002", "Material per nova oficina - urgent", 939.96m, 892.96m },
                    { 3, true, 1, new DateTime(2025, 2, 7, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 6, 10, 0, 0, 0, DateTimeKind.Utc), null, null, 10m, 2, 90.00m, "COM-2025-000003", "Mobiliari per sala de reunions", 899.95m, 809.95m },
                    { 4, true, 1, new DateTime(2025, 2, 12, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 11, 10, 0, 0, 0, DateTimeKind.Utc), null, null, 0m, 3, 0m, "COM-2025-000004", "Equipament informàtic complet per 3 treballadors", 625.93m, 625.93m },
                    { 5, true, 1, new DateTime(2025, 2, 17, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 16, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 21, 10, 0, 0, 0, DateTimeKind.Utc), null, 5m, 5, 55.00m, "COM-2025-000005", "Comanda completada i lliurada. Client satisfet.", 1099.94m, 1044.94m }
                });

            migrationBuilder.InsertData(
                table: "LiniesComanda",
                columns: new[] { "Id", "ArticleId", "ComandaId", "DataCreacio", "DescomptePercentatge", "Descripcio", "ImportDescompte", "NomProducte", "Ordre", "PreuUnitari", "Quantitat", "Subtotal", "Total" },
                values: new object[,]
                {
                    { 1, 2, 1, new DateTime(2025, 2, 1, 10, 0, 0, 0, DateTimeKind.Utc), 0m, "Ratolí òptic sense fils amb sensor de precisió", 0m, "Ratolí sense fils", 0, 25.50m, 5m, 127.50m, 127.50m },
                    { 2, 10, 1, new DateTime(2025, 2, 1, 10, 0, 0, 0, DateTimeKind.Utc), 0m, "Càmera web Full HD 1080p amb micròfon integrat", 0m, "Webcam HD", 1, 45.99m, 3m, 137.97m, 137.97m },
                    { 3, 6, 1, new DateTime(2025, 2, 1, 10, 0, 0, 0, DateTimeKind.Utc), 10m, "Impressora làser monocrom amb connexió Wi-Fi", 9.00m, "Impressora làser", 2, 89.99m, 1m, 89.99m, 80.99m },
                    { 4, 3, 1, new DateTime(2025, 2, 1, 10, 0, 0, 0, DateTimeKind.Utc), 5m, "Teclat mecànic retroil·luminat amb switches Cherry MX", 12.00m, "Teclat mecànic", 3, 120.00m, 2m, 240.00m, 228.00m },
                    { 5, 4, 2, new DateTime(2025, 2, 3, 10, 0, 0, 0, DateTimeKind.Utc), 0m, "Monitor LED Full HD 1920x1080 amb connexió HDMI", 0m, "Monitor 24 polzades", 0, 189.99m, 4m, 759.96m, 759.96m },
                    { 6, 7, 2, new DateTime(2025, 2, 3, 10, 0, 0, 0, DateTimeKind.Utc), 0m, "Disc dur extern USB 3.0 de 1TB per còpies de seguretat", 0m, "Disc dur extern 1TB", 1, 59.99m, 3m, 179.97m, 179.97m },
                    { 7, 5, 3, new DateTime(2025, 2, 6, 10, 0, 0, 0, DateTimeKind.Utc), 0m, "Cadira ergonòmica amb suport lumbar ajustable", 0m, "Cadira d'oficina", 0, 149.99m, 4m, 599.96m, 599.96m },
                    { 8, 8, 3, new DateTime(2025, 2, 6, 10, 0, 0, 0, DateTimeKind.Utc), 0m, "Taula d'oficina de fusta amb calaixos i organitzador", 0m, "Taula d'oficina", 1, 299.99m, 1m, 299.99m, 299.99m },
                    { 9, 2, 4, new DateTime(2025, 2, 11, 10, 0, 0, 0, DateTimeKind.Utc), 0m, "Ratolí òptic sense fils amb sensor de precisió", 0m, "Ratolí sense fils", 0, 25.50m, 3m, 76.50m, 76.50m },
                    { 10, 3, 4, new DateTime(2025, 2, 11, 10, 0, 0, 0, DateTimeKind.Utc), 0m, "Teclat mecànic retroil·luminat amb switches Cherry MX", 0m, "Teclat mecànic", 1, 120.00m, 3m, 360.00m, 360.00m },
                    { 11, 9, 4, new DateTime(2025, 2, 11, 10, 0, 0, 0, DateTimeKind.Utc), 5m, "Auriculars inalàmbrics amb cancel·lació de soroll", 12.00m, "Auriculars Bluetooth", 2, 79.99m, 3m, 239.97m, 227.97m },
                    { 12, 4, 5, new DateTime(2025, 2, 16, 10, 0, 0, 0, DateTimeKind.Utc), 0m, "Monitor LED Full HD 1920x1080 amb connexió HDMI", 0m, "Monitor 24 polzades", 0, 189.99m, 2m, 379.98m, 379.98m },
                    { 13, 8, 5, new DateTime(2025, 2, 16, 10, 0, 0, 0, DateTimeKind.Utc), 0m, "Taula d'oficina de fusta amb calaixos i organitzador", 0m, "Taula d'oficina", 1, 299.99m, 2m, 599.98m, 599.98m },
                    { 14, 7, 5, new DateTime(2025, 2, 16, 10, 0, 0, 0, DateTimeKind.Utc), 0m, "Disc dur extern USB 3.0 de 1TB per còpies de seguretat", 0m, "Disc dur extern 1TB", 2, 59.99m, 2m, 119.98m, 119.98m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "LiniesComanda",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
