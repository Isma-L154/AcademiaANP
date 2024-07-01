using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class ComentariosAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoUsuarioId",
                table: "Comentarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_CodigoUsuarioId",
                table: "Comentarios",
                column: "CodigoUsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Users_CodigoUsuarioId",
                table: "Comentarios",
                column: "CodigoUsuarioId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Users_CodigoUsuarioId",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_CodigoUsuarioId",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "CodigoUsuarioId",
                table: "Comentarios");
        }
    }
}
