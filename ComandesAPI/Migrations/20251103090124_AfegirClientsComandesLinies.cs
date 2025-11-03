using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComandesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AfegirClientsComandesLinies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuariId = table.Column<int>(type: "int", nullable: false),
                    NomEmpresa = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NIF = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adreca = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Poblacio = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Provincia = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodiPostal = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pais = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefon = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Actiu = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    DataCreacio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataModificacio = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Comandes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroComanda = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Estat = table.Column<int>(type: "int", nullable: false),
                    DataCreacio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataModificacio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataAprovacio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataFinalitzacio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Observacions = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DescomptePercentatge = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ImportDescompte = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalAmbDescompte = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Actiu = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comandes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LiniesComanda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ComandaId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: true),
                    NomProducte = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcio = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantitat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PreuUnitari = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DescomptePercentatge = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ImportDescompte = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Ordre = table.Column<int>(type: "int", nullable: false),
                    DataCreacio = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiniesComanda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiniesComanda_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LiniesComanda_Comandes_ComandaId",
                        column: x => x.ComandaId,
                        principalTable: "Comandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_NIF",
                table: "Clients",
                column: "NIF");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UsuariId",
                table: "Clients",
                column: "UsuariId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comandes_ClientId",
                table: "Comandes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Comandes_DataCreacio",
                table: "Comandes",
                column: "DataCreacio");

            migrationBuilder.CreateIndex(
                name: "IX_Comandes_Estat",
                table: "Comandes",
                column: "Estat");

            migrationBuilder.CreateIndex(
                name: "IX_Comandes_NumeroComanda",
                table: "Comandes",
                column: "NumeroComanda",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiniesComanda_ArticleId",
                table: "LiniesComanda",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_LiniesComanda_ComandaId",
                table: "LiniesComanda",
                column: "ComandaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiniesComanda");

            migrationBuilder.DropTable(
                name: "Comandes");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
