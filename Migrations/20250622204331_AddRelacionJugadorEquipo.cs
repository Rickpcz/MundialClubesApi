using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MundialClubesApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRelacionJugadorEquipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquipoId",
                table: "Jugadores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Jugadores_EquipoId",
                table: "Jugadores",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_LigaId",
                table: "Equipos",
                column: "LigaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipos_Ligas_LigaId",
                table: "Equipos",
                column: "LigaId",
                principalTable: "Ligas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jugadores_Equipos_EquipoId",
                table: "Jugadores",
                column: "EquipoId",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipos_Ligas_LigaId",
                table: "Equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Jugadores_Equipos_EquipoId",
                table: "Jugadores");

            migrationBuilder.DropIndex(
                name: "IX_Jugadores_EquipoId",
                table: "Jugadores");

            migrationBuilder.DropIndex(
                name: "IX_Equipos_LigaId",
                table: "Equipos");

            migrationBuilder.DropColumn(
                name: "EquipoId",
                table: "Jugadores");
        }
    }
}
