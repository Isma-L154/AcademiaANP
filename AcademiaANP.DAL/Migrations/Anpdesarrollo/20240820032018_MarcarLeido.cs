using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class MarcarLeido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsLeida",
                table: "Clase");

            migrationBuilder.CreateTable(
                name: "ClaseVista",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdClase = table.Column<int>(type: "int", nullable: false),
                    FechaVista = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaseVista", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaseVista_Clase_IdClase",
                        column: x => x.IdClase,
                        principalTable: "Clase",
                        principalColumn: "IdClase",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaseVista_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaseVista_IdClase",
                table: "ClaseVista",
                column: "IdClase");

            migrationBuilder.CreateIndex(
                name: "IX_ClaseVista_UserId",
                table: "ClaseVista",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaseVista");

            migrationBuilder.AddColumn<bool>(
                name: "EsLeida",
                table: "Clase",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
