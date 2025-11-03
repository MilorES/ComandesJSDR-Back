using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComandesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddJwtAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Estoc",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Estoc",
                value: 10);
        }
    }
}
