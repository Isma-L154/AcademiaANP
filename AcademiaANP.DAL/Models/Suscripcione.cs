using System;
using System.Collections.Generic;

namespace AcademiaANP.DAL.Models;

public partial class Suscripcione
{
    public int IdSuscripcion { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public virtual ICollection<Facturacion> Facturacions { get; set; } = new List<Facturacion>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
