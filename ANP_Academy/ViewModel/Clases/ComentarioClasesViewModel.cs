using ANP_Academy.DAL.Models;

namespace ANP_Academy.ViewModel.Clases
{
    public class ComentarioClasesViewModel
    {
        public Clase Clase { get; set; }
        public IEnumerable<ClaseComentario> Comentarios { get; set; }
        public ClaseComentario NuevoComentario { get; set; }
        public IEnumerable<ClaseXComentarios> ComentariosClases { get; set; }
    }
}
