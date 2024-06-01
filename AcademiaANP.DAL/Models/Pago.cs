using System;
using System.Collections.Generic;

namespace AcademiaANP.DAL.Models;

public partial class Pago
{
    public int IdPagos { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public bool? EstadoPago { get; set; }

    public decimal? Cantidad { get; set; }

    public string? IdUser { get; set; }

    public int? IdSuscripcion { get; set; }

    public virtual ICollection<Facturacion> Facturacions { get; set; } = new List<Facturacion>();

    public virtual Suscripcione? IdSuscripcionNavigation { get; set; }

    public virtual Usuario? IdUserNavigation { get; set; }
}
