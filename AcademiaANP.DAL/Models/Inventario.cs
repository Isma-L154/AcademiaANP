using ANP_Academy.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANP_Academy.DAL.Models;

public partial class Inventario
{
    [Key]
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public DateOnly FechaPedido { get; set; }

    public DateOnly FechaRecepcion { get; set; }

    public DateOnly? FechaCaducidad { get; set; }

    public bool? Estado { get; set; }

    public string Unidad { get; set; } = null!;

    [Column("Id_Categoria")]
    public int? IdCategoria { get; set; }

    [Column("Id_Proveedor")]
    public int? IdProveedor { get; set; }

    [Column("Id_Ubicacion")]
    public int? IdUbicacion { get; set; }

    public virtual Categoria? IdCategoriaNavigation { get; set; }

    public virtual Proveedor? IdProveedorNavigation { get; set; }

    public virtual Ubicacion? IdUbicacionNavigation { get; set; }
}
