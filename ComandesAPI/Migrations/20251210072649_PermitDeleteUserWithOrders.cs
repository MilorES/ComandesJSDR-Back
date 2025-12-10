using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComandesAPI.Migrations
{
    /// <inheritdoc />
    public partial class PermitDeleteUserWithOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comandes_Usuaris_UsuariId",
                table: "Comandes");

            migrationBuilder.AlterColumn<int>(
                name: "UsuariId",
                table: "Comandes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Comandes_Usuaris_UsuariId",
                table: "Comandes",
                column: "UsuariId",
                principalTable: "Usuaris",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comandes_Usuaris_UsuariId",
                table: "Comandes");

            migrationBuilder.AlterColumn<int>(
                name: "UsuariId",
                table: "Comandes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comandes_Usuaris_UsuariId",
                table: "Comandes",
                column: "UsuariId",
                principalTable: "Usuaris",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
