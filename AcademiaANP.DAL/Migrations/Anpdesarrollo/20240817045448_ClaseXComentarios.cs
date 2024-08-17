using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class ClaseXComentarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaseComentario",
                columns: table => new
                {
                    IdComentario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContenidoComentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaComentario = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodigoUsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaseComentario", x => x.IdComentario);
                    table.ForeignKey(
                        name: "FK_ClaseComentario_Users_CodigoUsuarioId",
                        column: x => x.CodigoUsuarioId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClaseXComentario",
                columns: table => new
                {
                    ClaseId = table.Column<int>(type: "int", nullable: false),
                    ComentarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaseXComentario", x => new { x.ClaseId, x.ComentarioId });
                    table.ForeignKey(
                        name: "FK_ClaseXComentario_ClaseComentario_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "ClaseComentario",
                        principalColumn: "IdComentario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaseXComentario_Clase_ClaseId",
                        column: x => x.ClaseId,
                        principalTable: "Clase",
                        principalColumn: "IdClase",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaseComentario_CodigoUsuarioId",
                table: "ClaseComentario",
                column: "CodigoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaseXComentario_ComentarioId",
                table: "ClaseXComentario",
                column: "ComentarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaseXComentario");

            migrationBuilder.DropTable(
                name: "ClaseComentario");
        }
    }
}
