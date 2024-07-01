using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class AgregarSolicitudes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Solicitudes",
                columns: table => new
                {
                    IdSolicitud = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdSuscripcion = table.Column<int>(type: "int", nullable: false),
                    Comprobante = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFinal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes", x => x.IdSolicitud);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Suscripciones_IdSuscripcion",
                        column: x => x.IdSuscripcion,
                        principalTable: "Suscripciones",
                        principalColumn: "Id_Suscripcion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_Id",
                table: "Solicitudes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_IdSuscripcion",
                table: "Solicitudes",
                column: "IdSuscripcion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Solicitudes");
        }
    }
}
