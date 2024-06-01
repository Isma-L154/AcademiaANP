using System;
using System.Collections.Generic;

namespace AcademiaANP.DAL.Models;

public partial class Comentario
{
    public int IdComentario { get; set; }

    public string Comentario1 { get; set; } = null!;

    public DateTime FechaComentario { get; set; }

    public virtual ICollection<Publicacione> Publicaciones { get; set; } = new List<Publicacione>();
}
