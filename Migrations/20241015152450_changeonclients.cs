using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimposioBack.Migrations
{
    /// <inheritdoc />
    public partial class changeonclients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    Id_Evento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_Evento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento", x => x.Id_Evento);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id_Cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero_Invitados = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Id_Evento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id_Cliente);
                    table.ForeignKey(
                        name: "FK_Cliente_Evento_Id_Evento",
                        column: x => x.Id_Evento,
                        principalTable: "Evento",
                        principalColumn: "Id_Evento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Id_Evento",
                table: "Cliente",
                column: "Id_Evento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Evento");
        }
    }
}
