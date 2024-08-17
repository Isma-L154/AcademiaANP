using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANP_Academy.DAL.Models
{
    public class ClaseComentario
    {
        [Key]
        public int IdComentario { get; set; }
        public string? ContenidoComentario { get; set; }
        public DateTime FechaComentario { get; set; }
        public string? CodigoUsuarioId { get; set; }
        public virtual ICollection<ClaseXComentarios> ClaseXComentario { get; set; } = new List<ClaseXComentarios>();
        public Usuario? CodigoUsuario { get; set; }
    }
}
