using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimposioBack.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Usuario",
                newName: "Id_Usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id_Usuario",
                table: "Usuario",
                newName: "Id");
        }
    }
}
