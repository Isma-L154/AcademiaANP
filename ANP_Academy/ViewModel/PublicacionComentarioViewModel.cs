using ANP_Academy.DAL.Models;

namespace ANP_Academy.ViewModel
{
    public class PublicacionComentarioViewModel
    {
        public IEnumerable<Publicacion> Publicaciones { get; set; }
        public IEnumerable<Comentario> Comentarios { get; set; }
        public Comentario NuevoComentario { get; set; }
        public IEnumerable<PublicacionComentario> ComentariosPubli { get; set; }
    }
}
