using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANP_Academy.DAL.Models
{
    [Table("Solicitudes")]
    public class Solicitudes
    {
        [Key]
        [Column("IdSolicitud")]
        public int IdSolicitud { get; set; }

        [Required]
        public string Id { get; set; } = null!;

        [ForeignKey("Id")]
        public Usuario User { get; set; } = null!;

        [Required]
        public int IdSuscripcion { get; set; }

        [ForeignKey("IdSuscripcion")]
        public Suscripcion SuscripcionEntity { get; set; } = null!;

        [Required]
        public byte[] Comprobante { get; set; } = null!;

        [Required]
        public DateTime FechaSolicitud { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFinal { get; set; }

        public bool? Estado { get; set; }
    }
}

