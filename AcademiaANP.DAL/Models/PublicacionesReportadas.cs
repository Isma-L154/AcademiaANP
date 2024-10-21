using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANP_Academy.DAL.Models
{
    public class PublicacionesReportadas
    {
        [Key]
        public int ReporteId { get; set; }
        [Required]
        public string? Motivo { get; set; }
        public string? Explicacion { get; set; }
        public string? CodigoUsuarioId { get; set; }
        public int PublicacionId { get; set; }
        public DateTime? Fecha { get; set; }
        public virtual Publicacion? Publicacion { get; set; }
        public virtual Usuario? CodigoUsuario { get; set; }
    }
}
