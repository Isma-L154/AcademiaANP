
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ANP_Academy.DAL.Models;

public partial class Comentario
{
    [Key]
    public int IdComentario { get; set; }
    public string? ContenidoComentario { get; set; }
    public DateTime FechaComentario { get; set; }
    public string? CodigoUsuarioId { get; set; }
    public virtual ICollection<PublicacionComentario> PublicacionComentario { get; set; } = new List<PublicacionComentario>();
    public Usuario? CodigoUsuario { get; set; }
}
