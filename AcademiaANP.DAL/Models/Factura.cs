using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANP_Academy.DAL.Models
{
    public class Factura
    {
        [Key]
        public int IdFactura { get; set; }

        public DateTime Fecha { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }

        public string? IdUser { get; set; }

        public int? IdSuscripcion { get; set; }

        public int? IdSolicitud { get; set; }

        [ForeignKey("IdUser")]
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey("IdSuscripcion")]
        public virtual Suscripcion? Suscripcion { get; set; }

        [ForeignKey("IdSolicitud")]
        public virtual Solicitudes? Solicitud { get; set; }
    }
}
