using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MundialClubesApi.Migrations
{
    /// <inheritdoc />
    public partial class AjustePartidoEstado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Partidos",
                newName: "Estado_Short");

            migrationBuilder.AddColumn<int>(
                name: "Estado_Elapsed",
                table: "Partidos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado_Long",
                table: "Partidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado_Elapsed",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "Estado_Long",
                table: "Partidos");

            migrationBuilder.RenameColumn(
                name: "Estado_Short",
                table: "Partidos",
                newName: "Estado");
        }
    }
}
