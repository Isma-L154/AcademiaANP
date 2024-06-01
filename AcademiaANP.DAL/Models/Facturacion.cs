using System.ComponentModel.DataAnnotations;

namespace ANP_Academy.DAL.Models;

public partial class Facturacion
{
    [Key]
    public int IdFactura { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Precio { get; set; }

    public int? IdPagos { get; set; }

    public string? IdUser { get; set; }

    public int? IdSuscripcion { get; set; }

    public virtual Pago? IdPagosNavigation { get; set; }

    public virtual Suscripcion? IdSuscripcionNavigation { get; set; }

    public virtual Usuario? IdUserNavigation { get; set; }
}
