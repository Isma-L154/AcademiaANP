using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANP_Academy.DAL.Models
{
    public class PublicacionComentario
    {
        public int PublicacionId { get; set; } 
        public int ComentarioId { get; set; }
        public virtual Publicacion? Publicacion { get; set; } 
        public virtual Comentario? Comentario { get; set; }
    }
}
