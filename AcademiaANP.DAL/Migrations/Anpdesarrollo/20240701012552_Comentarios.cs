using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class Comentarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Publicaci__Id_Co__14270015",
                table: "Publicaciones");

            migrationBuilder.DropIndex(
                name: "IX_Publicaciones_Id_Comentario",
                table: "Publicaciones");

            migrationBuilder.DropColumn(
                name: "Id_Comentario",
                table: "Publicaciones");

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "Publicaciones",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PublicacionComentarios",
                columns: table => new
                {
                    PublicacionId = table.Column<int>(type: "int", nullable: false),
                    ComentarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicacionComentarios", x => new { x.PublicacionId, x.ComentarioId });
                    table.ForeignKey(
                        name: "FK_PublicacionComentarios_Comentarios_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "Comentarios",
                        principalColumn: "Id_Comentario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicacionComentarios_Publicaciones_PublicacionId",
                        column: x => x.PublicacionId,
                        principalTable: "Publicaciones",
                        principalColumn: "Id_Publicacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicacionComentarios_ComentarioId",
                table: "PublicacionComentarios",
                column: "ComentarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicacionComentarios");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "Publicaciones");

            migrationBuilder.AddColumn<int>(
                name: "Id_Comentario",
                table: "Publicaciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_Id_Comentario",
                table: "Publicaciones",
                column: "Id_Comentario");

            migrationBuilder.AddForeignKey(
                name: "FK__Publicaci__Id_Co__14270015",
                table: "Publicaciones",
                column: "Id_Comentario",
                principalTable: "Comentarios",
                principalColumn: "Id_Comentario");
        }
    }
}
