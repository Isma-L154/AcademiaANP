using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class AddTabladeFactura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "MultimediaSize",
                table: "Publicaciones",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    IdFactura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdSuscripcion = table.Column<int>(type: "int", nullable: true),
                    IdSolicitud = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.IdFactura);
                    table.ForeignKey(
                        name: "FK_Facturas_Solicitudes_IdSolicitud",
                        column: x => x.IdSolicitud,
                        principalTable: "Solicitudes",
                        principalColumn: "Id_Solicitud");
                    table.ForeignKey(
                        name: "FK_Facturas_Suscripciones_IdSuscripcion",
                        column: x => x.IdSuscripcion,
                        principalTable: "Suscripciones",
                        principalColumn: "Id_Suscripcion");
                    table.ForeignKey(
                        name: "FK_Facturas_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_IdSolicitud",
                table: "Facturas",
                column: "IdSolicitud");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_IdSuscripcion",
                table: "Facturas",
                column: "IdSuscripcion");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_IdUser",
                table: "Facturas",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.AlterColumn<long>(
                name: "MultimediaSize",
                table: "Publicaciones",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
