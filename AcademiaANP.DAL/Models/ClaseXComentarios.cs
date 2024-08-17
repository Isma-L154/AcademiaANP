using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANP_Academy.DAL.Models
{
    public class ClaseXComentarios
    {
        public int ClaseId { get; set; }
        public int ComentarioId { get; set; }
        public virtual ClaseComentario? Comentario { get; set; }
        public virtual Clase? Clase { get; set; }
    }
}
