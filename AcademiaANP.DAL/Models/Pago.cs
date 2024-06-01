using ANP_Academy.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ANP_Academy.DAL.Models;

public partial class Pago
{
    [Key]
    public int IdPagos { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public bool? EstadoPago { get; set; }

    public decimal? Cantidad { get; set; }

    public string? IdUser { get; set; }

    public int? IdSuscripcion { get; set; }

    public virtual ICollection<Facturacion> Facturacions { get; set; } = new List<Facturacion>();

    public virtual Suscripcion? IdSuscripcionNavigation { get; set; }

    public virtual Usuario? IdUserNavigation { get; set; }
}
