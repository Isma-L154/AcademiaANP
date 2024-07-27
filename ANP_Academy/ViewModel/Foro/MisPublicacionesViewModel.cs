using ANP_Academy.DAL.Models;

namespace ANP_Academy.ViewModel.Foro
{
    public class MisPublicacionesViewModel
    {
        public IEnumerable<Publicacion> Publicaciones { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
