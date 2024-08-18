using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANP_Academy.DAL.Models
{
    [Table("Notificaciones")]
    public class Notificacion
    {
        [Key]
        public int IdNotificacion { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public required string IdUser { get; set; }

        [Required]
        public required string Contenido { get; set; }

        [Required]
        public required string TipoContenido { get; set; }

        public bool EsLeido { get; set; } = false;

        public virtual Usuario Usuario { get; set; }
    }
}

