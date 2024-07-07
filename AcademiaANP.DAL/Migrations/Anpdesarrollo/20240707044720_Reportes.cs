using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class Reportes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "PublicacionesReportadas",
                columns: table => new
                {
                    ReporteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Motivo = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Explicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoUsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PublicacionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicacionesReportadas", x => x.ReporteId);
                    table.ForeignKey(
                        name: "FK_PublicacionesReportadas_Publicaciones_PublicacionId",
                        column: x => x.PublicacionId,
                        principalTable: "Publicaciones",
                        principalColumn: "Id_Publicacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicacionesReportadas_Users_CodigoUsuarioId",
                        column: x => x.CodigoUsuarioId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicacionesReportadas_CodigoUsuarioId",
                table: "PublicacionesReportadas",
                column: "CodigoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicacionesReportadas_PublicacionId",
                table: "PublicacionesReportadas",
                column: "PublicacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicacionesReportadas");

        }
    }
}
