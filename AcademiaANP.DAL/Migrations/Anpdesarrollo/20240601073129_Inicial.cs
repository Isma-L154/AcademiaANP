using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademiaANP.DAL.Migrations.Anpdesarrollo
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id_Categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__CB90334986A82F6B", x => x.Id_Categoria);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id_Comentario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comentario = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Fecha_comentario = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comentar__5B4FE56FFC9D54DE", x => x.Id_Comentario);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id_Proveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proveedo__477B858ED514B108", x => x.Id_Proveedor);
                });

            migrationBuilder.CreateTable(
                name: "Suscripciones",
                columns: table => new
                {
                    Id_Suscripcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Suscripc__C0583DB0622B2151", x => x.Id_Suscripcion);
                });

            migrationBuilder.CreateTable(
                name: "Ubicacion",
                columns: table => new
                {
                    Id_Ubicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ubicacio__8368C3C32B715EA7", x => x.Id_Ubicacion);
                });

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Fecha_pedido = table.Column<DateOnly>(type: "date", nullable: false),
                    Fecha_recepcion = table.Column<DateOnly>(type: "date", nullable: false),
                    Fecha_caducidad = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    Unidad = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id_Categoria = table.Column<int>(type: "int", nullable: true),
                    Id_Proveedor = table.Column<int>(type: "int", nullable: true),
                    Id_Ubicacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inventar__3214EC07F651C35B", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Inventari__Id_Ca__09A971A2",
                        column: x => x.Id_Categoria,
                        principalTable: "Categorias",
                        principalColumn: "Id_Categoria");
                    table.ForeignKey(
                        name: "FK__Inventari__Id_Pr__0A9D95DB",
                        column: x => x.Id_Proveedor,
                        principalTable: "Proveedores",
                        principalColumn: "Id_Proveedor");
                    table.ForeignKey(
                        name: "FK__Inventari__Id_Ub__0B91BA14",
                        column: x => x.Id_Ubicacion,
                        principalTable: "Ubicacion",
                        principalColumn: "Id_Ubicacion");
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id_Pagos = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Estado_Pago = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    Id_User = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Id_Suscripcion = table.Column<int>(type: "int", nullable: true),
                    IdUserNavigationId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pagos__92395AA30149D604", x => x.Id_Pagos);
                    table.ForeignKey(
                        name: "FK_Pagos_Users_IdUserNavigationId",
                        column: x => x.IdUserNavigationId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Pagos__Id_Suscri__1BC821DD",
                        column: x => x.Id_Suscripcion,
                        principalTable: "Suscripciones",
                        principalColumn: "Id_Suscripcion");
                });

            migrationBuilder.CreateTable(
                name: "Publicaciones",
                columns: table => new
                {
                    Id_Publicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Fecha_Publicacion = table.Column<DateOnly>(type: "date", nullable: false),
                    Id_Comentario = table.Column<int>(type: "int", nullable: true),
                    Id_User = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IdUserNavigationId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Publicac__BE87075764D95DCC", x => x.Id_Publicacion);
                    table.ForeignKey(
                        name: "FK_Publicaciones_Users_IdUserNavigationId",
                        column: x => x.IdUserNavigationId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Publicaci__Id_Co__14270015",
                        column: x => x.Id_Comentario,
                        principalTable: "Comentarios",
                        principalColumn: "Id_Comentario");
                });

            migrationBuilder.CreateTable(
                name: "Facturacion",
                columns: table => new
                {
                    Id_Factura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Id_Pagos = table.Column<int>(type: "int", nullable: true),
                    Id_User = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Id_Suscripcion = table.Column<int>(type: "int", nullable: true),
                    IdUserNavigationId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Facturac__A8B88C226D095A3C", x => x.Id_Factura);
                    table.ForeignKey(
                        name: "FK_Facturacion_Users_IdUserNavigationId",
                        column: x => x.IdUserNavigationId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Facturaci__Id_Pa__208CD6FA",
                        column: x => x.Id_Pagos,
                        principalTable: "Pagos",
                        principalColumn: "Id_Pagos");
                    table.ForeignKey(
                        name: "FK__Facturaci__Id_Su__1F98B2C1",
                        column: x => x.Id_Suscripcion,
                        principalTable: "Suscripciones",
                        principalColumn: "Id_Suscripcion");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturacion_Id_Pagos",
                table: "Facturacion",
                column: "Id_Pagos");

            migrationBuilder.CreateIndex(
                name: "IX_Facturacion_Id_Suscripcion",
                table: "Facturacion",
                column: "Id_Suscripcion");

            migrationBuilder.CreateIndex(
                name: "IX_Facturacion_IdUserNavigationId",
                table: "Facturacion",
                column: "IdUserNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_Id_Categoria",
                table: "Inventario",
                column: "Id_Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_Id_Proveedor",
                table: "Inventario",
                column: "Id_Proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_Id_Ubicacion",
                table: "Inventario",
                column: "Id_Ubicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_Id_Suscripcion",
                table: "Pagos",
                column: "Id_Suscripcion");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdUserNavigationId",
                table: "Pagos",
                column: "IdUserNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_Id_Comentario",
                table: "Publicaciones",
                column: "Id_Comentario");

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_IdUserNavigationId",
                table: "Publicaciones",
                column: "IdUserNavigationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facturacion");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "Publicaciones");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Ubicacion");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Suscripciones");
        }
    }
}
