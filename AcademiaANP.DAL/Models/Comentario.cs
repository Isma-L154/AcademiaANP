
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ANP_Academy.DAL.Models;

public partial class Comentario
{
    [Key]
    public int IdComentario { get; set; }

    public string Comentario1 { get; set; } = null!;

    public DateTime FechaComentario { get; set; }

    public virtual ICollection<Publicacion> Publicaciones { get; set; } = new List<Publicacion>();
}
