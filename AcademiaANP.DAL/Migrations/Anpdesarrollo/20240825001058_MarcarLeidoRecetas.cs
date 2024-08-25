using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class MarcarLeidoRecetas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecetaVista",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdReceta = table.Column<int>(type: "int", nullable: false),
                    FechaVista = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecetaVista", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecetaVista_Receta_IdReceta",
                        column: x => x.IdReceta,
                        principalTable: "Receta",
                        principalColumn: "IdReceta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecetaVista_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecetaVista_IdReceta",
                table: "RecetaVista",
                column: "IdReceta");

            migrationBuilder.CreateIndex(
                name: "IX_RecetaVista_UserId",
                table: "RecetaVista",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecetaVista");
        }
    }
}
