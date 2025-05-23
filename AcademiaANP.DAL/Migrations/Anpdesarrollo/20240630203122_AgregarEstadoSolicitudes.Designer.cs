﻿// <auto-generated />
using System;
using ANP_Academy.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ANP_Academy.DAL.Migrations.Anpdesarrollo
{
    [DbContext(typeof(AnpdesarrolloContext))]
    [Migration("20240630203122_AgregarEstadoSolicitudes")]
    partial class AgregarEstadoSolicitudes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ANP_Academy.DAL.Models.Categoria", b =>
                {
                    b.Property<int>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Categoria");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCategoria"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("varchar(256)");

                    b.HasKey("IdCategoria")
                        .HasName("PK__Categori__CB90334986A82F6B");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Comentario", b =>
                {
                    b.Property<int>("IdComentario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Comentario");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdComentario"));

                    b.Property<string>("Comentario1")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("Comentario");

                    b.Property<DateTime>("FechaComentario")
                        .HasColumnType("datetime")
                        .HasColumnName("Fecha_comentario");

                    b.HasKey("IdComentario")
                        .HasName("PK__Comentar__5B4FE56FFC9D54DE");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Facturacion", b =>
                {
                    b.Property<int>("IdFactura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Factura");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdFactura"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime");

                    b.Property<int?>("IdPagos")
                        .HasColumnType("int")
                        .HasColumnName("Id_Pagos");

                    b.Property<int?>("IdSuscripcion")
                        .HasColumnType("int")
                        .HasColumnName("Id_Suscripcion");

                    b.Property<string>("IdUser")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Id_User");

                    b.Property<string>("IdUserNavigationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18, 0)");

                    b.HasKey("IdFactura")
                        .HasName("PK__Facturac__A8B88C226D095A3C");

                    b.HasIndex("IdPagos");

                    b.HasIndex("IdSuscripcion");

                    b.HasIndex("IdUserNavigationId");

                    b.ToTable("Facturacion", (string)null);
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Inventario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("varchar(256)");

                    b.Property<bool?>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateOnly?>("FechaCaducidad")
                        .HasColumnType("date")
                        .HasColumnName("Fecha_caducidad");

                    b.Property<DateOnly>("FechaPedido")
                        .HasColumnType("date")
                        .HasColumnName("Fecha_pedido");

                    b.Property<DateOnly>("FechaRecepcion")
                        .HasColumnType("date")
                        .HasColumnName("Fecha_recepcion");

                    b.Property<int?>("IdCategoria")
                        .HasColumnType("int")
                        .HasColumnName("Id_Categoria");

                    b.Property<int?>("IdProveedor")
                        .HasColumnType("int")
                        .HasColumnName("Id_Proveedor");

                    b.Property<int?>("IdUbicacion")
                        .HasColumnType("int")
                        .HasColumnName("Id_Ubicacion");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18, 0)");

                    b.Property<string>("Unidad")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id")
                        .HasName("PK__Inventar__3214EC07F651C35B");

                    b.HasIndex("IdCategoria");

                    b.HasIndex("IdProveedor");

                    b.HasIndex("IdUbicacion");

                    b.ToTable("Inventario", (string)null);
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Pago", b =>
                {
                    b.Property<int>("IdPagos")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Pagos");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPagos"));

                    b.Property<decimal?>("Cantidad")
                        .HasColumnType("decimal(18, 0)");

                    b.Property<bool?>("EstadoPago")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("Estado_Pago");

                    b.Property<int?>("IdSuscripcion")
                        .HasColumnType("int")
                        .HasColumnName("Id_Suscripcion");

                    b.Property<string>("IdUser")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Id_User");

                    b.Property<string>("IdUserNavigationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(false)
                        .HasColumnType("varchar(128)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18, 0)");

                    b.HasKey("IdPagos")
                        .HasName("PK__Pagos__92395AA30149D604");

                    b.HasIndex("IdSuscripcion");

                    b.HasIndex("IdUserNavigationId");

                    b.ToTable("Pagos");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Proveedor", b =>
                {
                    b.Property<int>("IdProveedor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Proveedor");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProveedor"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("varchar(256)");

                    b.HasKey("IdProveedor")
                        .HasName("PK__Proveedo__477B858ED514B108");

                    b.ToTable("Proveedores");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Publicacion", b =>
                {
                    b.Property<int>("IdPublicacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Publicacion");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPublicacion"));

                    b.Property<string>("CodigoUsuarioId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Id_User");

                    b.Property<string>("Descripcion")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<DateOnly>("FechaPublicacion")
                        .HasColumnType("date")
                        .HasColumnName("Fecha_Publicacion");

                    b.Property<int?>("IdComentario")
                        .HasColumnType("int")
                        .HasColumnName("Id_Comentario");

                    b.HasKey("IdPublicacion")
                        .HasName("PK__Publicac__BE87075764D95DCC");

                    b.HasIndex("CodigoUsuarioId");

                    b.HasIndex("IdComentario");

                    b.ToTable("Publicaciones");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Solicitudes", b =>
                {
                    b.Property<int>("IdSolicitud")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Solicitud");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSolicitud"));

                    b.Property<byte[]>("Comprobante")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("FechaFinal")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("FechaInicio")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("FechaSolicitud")
                        .HasColumnType("datetime");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("IdSuscripcion")
                        .HasColumnType("int");

                    b.HasKey("IdSolicitud")
                        .HasName("PK__Solicitudes__123456789");

                    b.HasIndex("Id");

                    b.HasIndex("IdSuscripcion");

                    b.ToTable("Solicitudes");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Suscripcion", b =>
                {
                    b.Property<int>("IdSuscripcion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Suscripcion");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSuscripcion"));

                    b.Property<int>("Duracion")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(false)
                        .HasColumnType("varchar(128)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18, 0)");

                    b.HasKey("IdSuscripcion")
                        .HasName("PK__Suscripc__C0583DB0622B2151");

                    b.ToTable("Suscripciones");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Ubicacion", b =>
                {
                    b.Property<int>("IdUbicacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Ubicacion");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUbicacion"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("varchar(256)");

                    b.HasKey("IdUbicacion")
                        .HasName("PK__Ubicacio__8368C3C32B715EA7");

                    b.ToTable("Ubicacion", (string)null);
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Usuario", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<bool?>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PrimApellido")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SegApellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SuscripcionId")
                        .HasColumnType("int");

                    b.Property<bool?>("Suscrito")
                        .HasColumnType("bit");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SuscripcionId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Facturacion", b =>
                {
                    b.HasOne("ANP_Academy.DAL.Models.Pago", "IdPagosNavigation")
                        .WithMany("Facturacions")
                        .HasForeignKey("IdPagos")
                        .HasConstraintName("FK__Facturaci__Id_Pa__208CD6FA");

                    b.HasOne("ANP_Academy.DAL.Models.Suscripcion", "IdSuscripcionNavigation")
                        .WithMany("Facturacions")
                        .HasForeignKey("IdSuscripcion")
                        .HasConstraintName("FK__Facturaci__Id_Su__1F98B2C1");

                    b.HasOne("ANP_Academy.DAL.Models.Usuario", "IdUserNavigation")
                        .WithMany()
                        .HasForeignKey("IdUserNavigationId");

                    b.Navigation("IdPagosNavigation");

                    b.Navigation("IdSuscripcionNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Inventario", b =>
                {
                    b.HasOne("ANP_Academy.DAL.Models.Categoria", "IdCategoriaNavigation")
                        .WithMany("Inventarios")
                        .HasForeignKey("IdCategoria")
                        .HasConstraintName("FK__Inventari__Id_Ca__09A971A2");

                    b.HasOne("ANP_Academy.DAL.Models.Proveedor", "IdProveedorNavigation")
                        .WithMany("Inventarios")
                        .HasForeignKey("IdProveedor")
                        .HasConstraintName("FK__Inventari__Id_Pr__0A9D95DB");

                    b.HasOne("ANP_Academy.DAL.Models.Ubicacion", "IdUbicacionNavigation")
                        .WithMany("Inventarios")
                        .HasForeignKey("IdUbicacion")
                        .HasConstraintName("FK__Inventari__Id_Ub__0B91BA14");

                    b.Navigation("IdCategoriaNavigation");

                    b.Navigation("IdProveedorNavigation");

                    b.Navigation("IdUbicacionNavigation");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Pago", b =>
                {
                    b.HasOne("ANP_Academy.DAL.Models.Suscripcion", "IdSuscripcionNavigation")
                        .WithMany("Pagos")
                        .HasForeignKey("IdSuscripcion")
                        .HasConstraintName("FK__Pagos__Id_Suscri__1BC821DD");

                    b.HasOne("ANP_Academy.DAL.Models.Usuario", "IdUserNavigation")
                        .WithMany()
                        .HasForeignKey("IdUserNavigationId");

                    b.Navigation("IdSuscripcionNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Publicacion", b =>
                {
                    b.HasOne("ANP_Academy.DAL.Models.Usuario", "CodigoUsuario")
                        .WithMany()
                        .HasForeignKey("CodigoUsuarioId");

                    b.HasOne("ANP_Academy.DAL.Models.Comentario", "IdComentarioNavigation")
                        .WithMany("Publicaciones")
                        .HasForeignKey("IdComentario")
                        .HasConstraintName("FK__Publicaci__Id_Co__14270015");

                    b.Navigation("CodigoUsuario");

                    b.Navigation("IdComentarioNavigation");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Solicitudes", b =>
                {
                    b.HasOne("ANP_Academy.DAL.Models.Usuario", "User")
                        .WithMany("Solicitudes")
                        .HasForeignKey("Id")
                        .IsRequired()
                        .HasConstraintName("FK_Solicitudes_Usuarios");

                    b.HasOne("ANP_Academy.DAL.Models.Suscripcion", "SuscripcionEntity")
                        .WithMany("Solicitudes")
                        .HasForeignKey("IdSuscripcion")
                        .IsRequired()
                        .HasConstraintName("FK_Solicitudes_Suscripciones");

                    b.Navigation("SuscripcionEntity");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Usuario", b =>
                {
                    b.HasOne("ANP_Academy.DAL.Models.Suscripcion", "Suscripcion")
                        .WithMany("Usuarios")
                        .HasForeignKey("SuscripcionId");

                    b.Navigation("Suscripcion");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Categoria", b =>
                {
                    b.Navigation("Inventarios");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Comentario", b =>
                {
                    b.Navigation("Publicaciones");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Pago", b =>
                {
                    b.Navigation("Facturacions");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Proveedor", b =>
                {
                    b.Navigation("Inventarios");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Suscripcion", b =>
                {
                    b.Navigation("Facturacions");

                    b.Navigation("Pagos");

                    b.Navigation("Solicitudes");

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Ubicacion", b =>
                {
                    b.Navigation("Inventarios");
                });

            modelBuilder.Entity("ANP_Academy.DAL.Models.Usuario", b =>
                {
                    b.Navigation("Solicitudes");
                });
#pragma warning restore 612, 618
        }
    }
}
