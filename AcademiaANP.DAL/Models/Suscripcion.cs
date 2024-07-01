using ANP_Academy.DAL.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ANP_Academy.DAL.Models;
public partial class Suscripcion
{
    [Key]
    public int IdSuscripcion { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Duracion { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Facturacion> Facturacions { get; set; } = new List<Facturacion>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<Solicitudes> Solicitudes { get; set; } = new List<Solicitudes>();
}
