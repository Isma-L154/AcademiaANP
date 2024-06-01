using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Cambio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimApellido",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SegApellido",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SuscripcionId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Suscrito",
                table: "Users",
                type: "bit",
                nullable: true);


            migrationBuilder.CreateIndex(
                name: "IX_Users_SuscripcionId",
                table: "Users",
                column: "SuscripcionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Suscripcion_SuscripcionId",
                table: "Users",
                column: "SuscripcionId",
                principalTable: "Suscripciones",
                principalColumn: "Id_Suscripcion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Suscripcion_SuscripcionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SuscripcionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PrimApellido",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SegApellido",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SuscripcionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Suscrito",
                table: "Users");
        }
    }
}
