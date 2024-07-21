namespace ANP_Academy.ViewModel
{
    public class ClaseViewModel
    {
        public int IdClase { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string URLVideo { get; set; } = null!;
        public IFormFile? Imagen { get; set; }
    }
}
