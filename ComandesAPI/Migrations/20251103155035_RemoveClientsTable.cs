using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComandesAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClientsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comandes_Clients_ClientId",
                table: "Comandes");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Comandes",
                newName: "UsuariId");

            migrationBuilder.RenameIndex(
                name: "IX_Comandes_ClientId",
                table: "Comandes",
                newName: "IX_Comandes_UsuariId");

            migrationBuilder.UpdateData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 1,
                column: "UsuariId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 2,
                column: "UsuariId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 3,
                column: "UsuariId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 4,
                column: "UsuariId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 5,
                column: "UsuariId",
                value: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_Comandes_Usuaris_UsuariId",
                table: "Comandes",
                column: "UsuariId",
                principalTable: "Usuaris",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comandes_Usuaris_UsuariId",
                table: "Comandes");

            migrationBuilder.RenameColumn(
                name: "UsuariId",
                table: "Comandes",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Comandes_UsuariId",
                table: "Comandes",
                newName: "IX_Comandes_ClientId");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuariId = table.Column<int>(type: "int", nullable: false),
                    Actiu = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Adreca = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodiPostal = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCreacio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataModificacio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NIF = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomEmpresa = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pais = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Poblacio = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Provincia = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefon = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Usuaris_UsuariId",
                        column: x => x.UsuariId,
                        principalTable: "Usuaris",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Actiu", "Adreca", "CodiPostal", "DataCreacio", "DataModificacio", "NIF", "NomEmpresa", "Notes", "Pais", "Poblacio", "Provincia", "Telefon", "UsuariId" },
                values: new object[] { 1, true, "Carrer de la Innovació, 42", "08028", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), null, "B12345678", "Tecnologies Avançades SL", "Client preferent amb descompte del 5% en compres superiors a 1000€", "Espanya", "Barcelona", "Barcelona", "+34 932 456 789", 2 });

            migrationBuilder.UpdateData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ClientId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 2,
                column: "ClientId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 3,
                column: "ClientId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 4,
                column: "ClientId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Comandes",
                keyColumn: "Id",
                keyValue: 5,
                column: "ClientId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_NIF",
                table: "Clients",
                column: "NIF");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UsuariId",
                table: "Clients",
                column: "UsuariId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comandes_Clients_ClientId",
                table: "Comandes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
