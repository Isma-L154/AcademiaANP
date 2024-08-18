using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ANP_Academy.DAL.Models
{
    public partial class AnpdesarrolloContext : DbContext
    {
        public AnpdesarrolloContext()
        {
        }

        public AnpdesarrolloContext(DbContextOptions<AnpdesarrolloContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer();
        }

        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Comentario> Comentarios { get; set; }
        public virtual DbSet<Facturacion> Facturacions { get; set; }
        public virtual DbSet<Inventario> Inventarios { get; set; }
        public virtual DbSet<Pago> Pagos { get; set; }
        public virtual DbSet<Proveedor> Proveedores { get; set; }
        public virtual DbSet<Publicacion> Publicaciones { get; set; }
        public virtual DbSet<Suscripcion> Suscripciones { get; set; }
        public virtual DbSet<Ubicacion> Ubicacions { get; set; }
        public virtual DbSet<Solicitudes> Solicitudes { get; set; }
        public virtual DbSet<PublicacionComentario> PublicacionComentarios { get; set; }
        public virtual DbSet<PublicacionesReportadas> PublicacionesReportadas { get; set; }
        public virtual DbSet<Factura> Facturas { get; set; }
        public virtual DbSet<Clase> Clases{ get; set; }
        public virtual DbSet<Receta> Recetas { get; set; }
        public virtual DbSet<RecetaArchivo> RecetaArchivos { get; set; }
        public virtual DbSet<ClaseComentario> ClaseComentario { get; set; }
        public virtual DbSet<ClaseXComentarios> ClaseXComentario { get; set; }
        public virtual DbSet<Notificacion> Notificaciones { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CB90334986A82F6B");

                entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(e => e.IdComentario).HasName("PK__Comentar__5B4FE56FFC9D54DE");

                entity.Property(e => e.IdComentario).HasColumnName("Id_Comentario");
                entity.Property(e => e.ContenidoComentario)
                    .IsUnicode(false)
                    .HasColumnName("Comentario");
                entity.Property(e => e.FechaComentario)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_comentario");
            });

            modelBuilder.Entity<PublicacionComentario>()
                       .HasKey(pc => new { pc.PublicacionId, pc.ComentarioId });

            modelBuilder.Entity<ClaseXComentarios>()
                       .HasKey(pc => new { pc.ClaseId, pc.ComentarioId });


            modelBuilder.Entity<Facturacion>(entity =>
            {
                entity.HasKey(e => e.IdFactura).HasName("PK__Facturac__A8B88C226D095A3C");

                entity.ToTable("Facturacion");

                entity.Property(e => e.IdFactura).HasColumnName("Id_Factura");
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.IdPagos).HasColumnName("Id_Pagos");
                entity.Property(e => e.IdSuscripcion).HasColumnName("Id_Suscripcion");
                entity.Property(e => e.IdUser)
                    .HasMaxLength(450)
                    .HasColumnName("Id_User");
                entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdPagosNavigation).WithMany(p => p.Facturacions)
                    .HasForeignKey(d => d.IdPagos)
                    .HasConstraintName("FK__Facturaci__Id_Pa__208CD6FA");

                entity.HasOne(d => d.IdSuscripcionNavigation).WithMany(p => p.Facturacions)
                    .HasForeignKey(d => d.IdSuscripcion)
                    .HasConstraintName("FK__Facturaci__Id_Su__1F98B2C1");
            });

            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Inventar__3214EC07F651C35B");

                entity.ToTable("Inventario");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(256)
                    .IsUnicode(false);
                entity.Property(e => e.Estado).HasDefaultValue(true);
                entity.Property(e => e.FechaCaducidad).HasColumnName("Fecha_caducidad");
                entity.Property(e => e.FechaPedido).HasColumnName("Fecha_pedido");
                entity.Property(e => e.FechaRecepcion).HasColumnName("Fecha_recepcion");
                entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");
                entity.Property(e => e.IdProveedor).HasColumnName("Id_Proveedor");
                entity.Property(e => e.IdUbicacion).HasColumnName("Id_Ubicacion");
                entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.Unidad)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK__Inventari__Id_Ca__09A971A2");

                entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.IdProveedor)
                    .HasConstraintName("FK__Inventari__Id_Pr__0A9D95DB");

                entity.HasOne(d => d.IdUbicacionNavigation).WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.IdUbicacion)
                    .HasConstraintName("FK__Inventari__Id_Ub__0B91BA14");
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.HasKey(e => e.IdPagos).HasName("PK__Pagos__92395AA30149D604");

                entity.Property(e => e.IdPagos).HasColumnName("Id_Pagos");
                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.EstadoPago)
                    .HasDefaultValue(false)
                    .HasColumnName("Estado_Pago");
                entity.Property(e => e.IdSuscripcion).HasColumnName("Id_Suscripcion");
                entity.Property(e => e.IdUser)
                    .HasMaxLength(450)
                    .HasColumnName("Id_User");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdSuscripcionNavigation).WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdSuscripcion)
                    .HasConstraintName("FK__Pagos__Id_Suscri__1BC821DD");
            });
            
            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__477B858ED514B108");

                entity.Property(e => e.IdProveedor).HasColumnName("Id_Proveedor");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Publicacion>(entity =>
            {
                entity.HasKey(e => e.IdPublicacion).HasName("PK__Publicac__BE87075764D95DCC");

            entity.Property(e => e.IdPublicacion).HasColumnName("Id_Publicacion");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.FechaPublicacion).HasColumnName("Fecha_Publicacion");
            entity.Property(e => e.CodigoUsuarioId)
                .HasMaxLength(450)
                .HasColumnName("Id_User");
        });

            modelBuilder.Entity<Solicitudes>(entity =>
            {
                entity.HasKey(e => e.IdSolicitud).HasName("PK__Solicitudes__123456789");

                entity.Property(e => e.IdSolicitud).HasColumnName("Id_Solicitud");
                entity.Property(e => e.Id).IsRequired().HasMaxLength(450);
                entity.Property(e => e.IdSuscripcion).IsRequired();
                entity.Property(e => e.Comprobante).IsRequired();
                entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
                entity.Property(e => e.FechaInicio).HasColumnType("datetime").IsRequired(false);
                entity.Property(e => e.FechaFinal).HasColumnType("datetime").IsRequired(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Solicitudes)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Solicitudes_Usuarios");

                entity.HasOne(d => d.SuscripcionEntity)
                    .WithMany(p => p.Solicitudes)
                    .HasForeignKey(d => d.IdSuscripcion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Solicitudes_Suscripciones");
            });

            modelBuilder.Entity<Suscripcion>(entity =>
            {
                entity.HasKey(e => e.IdSuscripcion).HasName("PK__Suscripc__C0583DB0622B2151");

                entity.Property(e => e.IdSuscripcion).HasColumnName("Id_Suscripcion");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(128)
                    .IsUnicode(false);
                entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Ubicacion>(entity =>
            {
                entity.HasKey(e => e.IdUbicacion).HasName("PK__Ubicacio__8368C3C32B715EA7");

                entity.ToTable("Ubicacion");

                entity.Property(e => e.IdUbicacion).HasColumnName("Id_Ubicacion");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Notificacion>(entity =>
            {
                entity.HasOne(n => n.Usuario)
                      .WithMany(u => u.Notificacion)
                      .HasForeignKey(n => n.IdUser);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
