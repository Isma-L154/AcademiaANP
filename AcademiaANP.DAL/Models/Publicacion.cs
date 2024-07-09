using ANP_Academy.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ANP_Academy.DAL.Models;

public partial class Publicacion
{
    [Key]
    public int IdPublicacion { get; set; }
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public DateOnly FechaPublicacion { get; set; }
    
    /*Propiedades para Multimedia*/
    public string? MultimediaData { get; set; }
    public string? MultimediaType { get; set; }
    public long? MultimediaSize { get; set; }
    public string? MultimediaName { get; set; }

    public string? CodigoUsuarioId { get; set; }
    public virtual ICollection<PublicacionComentario> PublicacionComentarios { get; set; } = new List<PublicacionComentario>();
    public  Usuario? CodigoUsuario { get; set; }
}
