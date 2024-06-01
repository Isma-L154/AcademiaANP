using System;
using System.Collections.Generic;

namespace AcademiaANP.DAL.Models;

public partial class Publicacione
{
    public int IdPublicacion { get; set; }

    public string? Descripcion { get; set; }

    public DateOnly FechaPublicacion { get; set; }

    public int? IdComentario { get; set; }

    public string? IdUser { get; set; }

    public virtual Comentario? IdComentarioNavigation { get; set; }

    public virtual Usuario? IdUserPrimary { get; set; }
}
