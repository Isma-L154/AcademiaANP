using ANP_Academy.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ANP_Academy.DAL.Models;

public partial class Publicacion
{
    [Key]
    public int IdPublicacion { get; set; }

    public string? Descripcion { get; set; }

    public DateOnly FechaPublicacion { get; set; }

    public int? IdComentario { get; set; }

    public string? CodigoUsuarioId { get; set; }

    public  Comentario? IdComentarioNavigation { get; set; }

    public  Usuario? CodigoUsuario { get; set; }
}
