using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class Multimedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MultimediaData",
                table: "Publicaciones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MultimediaName",
                table: "Publicaciones",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MultimediaSize",
                table: "Publicaciones",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MultimediaType",
                table: "Publicaciones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MultimediaData",
                table: "Publicaciones");

            migrationBuilder.DropColumn(
                name: "MultimediaName",
                table: "Publicaciones");

            migrationBuilder.DropColumn(
                name: "MultimediaSize",
                table: "Publicaciones");

            migrationBuilder.DropColumn(
                name: "MultimediaType",
                table: "Publicaciones");
        }
    }
}
