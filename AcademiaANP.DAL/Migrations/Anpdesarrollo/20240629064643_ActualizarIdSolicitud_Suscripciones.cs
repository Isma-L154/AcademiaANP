using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class ActualizarIdSolicitud_Suscripciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Suscripciones_IdSuscripcion",
                table: "Solicitudes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Users_Id",
                table: "Solicitudes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Solicitudes",
                table: "Solicitudes");

            migrationBuilder.RenameColumn(
                name: "IdSolicitud",
                table: "Solicitudes",
                newName: "Id_Solicitud");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaSolicitud",
                table: "Solicitudes",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInicio",
                table: "Solicitudes",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaFinal",
                table: "Solicitudes",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Solicitudes__123456789",
                table: "Solicitudes",
                column: "Id_Solicitud");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Suscripciones",
                table: "Solicitudes",
                column: "IdSuscripcion",
                principalTable: "Suscripciones",
                principalColumn: "Id_Suscripcion");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Usuarios",
                table: "Solicitudes",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Suscripciones",
                table: "Solicitudes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Usuarios",
                table: "Solicitudes");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Solicitudes__123456789",
                table: "Solicitudes");

            migrationBuilder.RenameColumn(
                name: "Id_Solicitud",
                table: "Solicitudes",
                newName: "IdSolicitud");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaSolicitud",
                table: "Solicitudes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInicio",
                table: "Solicitudes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaFinal",
                table: "Solicitudes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Solicitudes",
                table: "Solicitudes",
                column: "IdSolicitud");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Suscripciones_IdSuscripcion",
                table: "Solicitudes",
                column: "IdSuscripcion",
                principalTable: "Suscripciones",
                principalColumn: "Id_Suscripcion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Users_Id",
                table: "Solicitudes",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
