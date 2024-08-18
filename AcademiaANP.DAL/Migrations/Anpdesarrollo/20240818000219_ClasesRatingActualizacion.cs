using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class ClasesRatingActualizacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaseRating");

            migrationBuilder.CreateTable(
                name: "ClaseRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClase = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaseRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaseRatings_Clase_IdClase",
                        column: x => x.IdClase,
                        principalTable: "Clase",
                        principalColumn: "IdClase",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaseRatings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaseRatings_IdClase",
                table: "ClaseRatings",
                column: "IdClase");

            migrationBuilder.CreateIndex(
                name: "IX_ClaseRatings_UserId",
                table: "ClaseRatings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaseRatings");

            migrationBuilder.CreateTable(
                name: "ClaseRating",
                columns: table => new
                {
                    IdRating = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClase = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaseRating", x => x.IdRating);
                    table.ForeignKey(
                        name: "FK_ClaseRating_Clase_IdClase",
                        column: x => x.IdClase,
                        principalTable: "Clase",
                        principalColumn: "IdClase",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaseRating_IdClase",
                table: "ClaseRating",
                column: "IdClase");
        }
    }
}
