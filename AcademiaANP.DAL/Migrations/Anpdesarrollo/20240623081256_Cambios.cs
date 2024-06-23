using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class Cambios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Id_User",
                table: "Publicaciones",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_Id_User",
                table: "Publicaciones",
                column: "Id_User");

            migrationBuilder.AddForeignKey(
                name: "FK_Publicaciones_Users_Id_User",
                table: "Publicaciones",
                column: "Id_User",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publicaciones_Users_Id_User",
                table: "Publicaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Suscripciones_SuscripcionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SuscripcionId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_IdUserNavigationId",
                table: "Publicaciones",
                column: "IdUserNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publicaciones_Users_IdUserNavigationId",
                table: "Publicaciones",
                column: "IdUserNavigationId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
