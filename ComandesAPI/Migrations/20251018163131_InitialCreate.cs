using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComandesAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcio = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Preu = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Estoc = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Actiu = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    DataCreacio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataModificacio = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuaris",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "User")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuaris", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Actiu", "Categoria", "DataCreacio", "DataModificacio", "Descripcio", "Estoc", "Nom", "Preu" },
                values: new object[,]
                {
                    { 1, true, "Informàtica", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ordinador portàtil per oficina amb pantalla de 15.6 polzades", 10, "Ordinador portàtil", 899.99m },
                    { 2, true, "Informàtica", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ratolí òptic sense fils amb sensor de precisió", 50, "Ratolí sense fils", 25.50m },
                    { 3, true, "Informàtica", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Teclat mecànic retroil·luminat amb switches Cherry MX", 25, "Teclat mecànic", 120.00m },
                    { 4, true, "Informàtica", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Monitor LED Full HD 1920x1080 amb connexió HDMI", 15, "Monitor 24 polzades", 189.99m },
                    { 5, true, "Mobiliari", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cadira ergonòmica amb suport lumbar ajustable", 8, "Cadira d'oficina", 149.99m },
                    { 6, true, "Informàtica", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Impressora làser monocrom amb connexió Wi-Fi", 12, "Impressora làser", 89.99m },
                    { 7, true, "Informàtica", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Disc dur extern USB 3.0 de 1TB per còpies de seguretat", 30, "Disc dur extern 1TB", 59.99m },
                    { 8, true, "Mobiliari", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Taula d'oficina de fusta amb calaixos i organitzador", 5, "Taula d'oficina", 299.99m },
                    { 9, true, "Àudio", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Auriculars inalàmbrics amb cancel·lació de soroll", 20, "Auriculars Bluetooth", 79.99m },
                    { 10, true, "Informàtica", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Càmera web Full HD 1080p amb micròfon integrat", 18, "Webcam HD", 45.99m }
                });

            migrationBuilder.InsertData(
                table: "Usuaris",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsEnabled", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@comandesjdsr.com", "Administrador del Sistema", true, "$2a$12$wKQgs3QYMJdHm791BDWZ7eJCndZsZAvQYcbBQ9UCEs.sFP6Hp1LOW", "Administrator", "administrador" },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "usuari@comandesjdsr.com", "Usuari Estàndard", true, "$2a$12$wKQgs3QYMJdHm791BDWZ7eJCndZsZAvQYcbBQ9UCEs.sFP6Hp1LOW", "User", "usuari" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Categoria",
                table: "Articles",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Nom",
                table: "Articles",
                column: "Nom");

            migrationBuilder.CreateIndex(
                name: "IX_Usuaris_Email",
                table: "Usuaris",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuaris_Username",
                table: "Usuaris",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Usuaris");
        }
    }
}
