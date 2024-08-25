using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ANP_Academy.ViewModel
{
    public class RecetaViewModel
    {
        public int IdReceta { get; set; }

        public string Titulo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public string URLVideo { get; set; } = null!;

        public IFormFile? Imagen { get; set; }

        public float Rating { get; set; }

        public bool EsLeida { get; set; }

        public List<RecetaArchivoViewModel>? ArchivosExistentes { get; set; } = new List<RecetaArchivoViewModel>();

        public List<IFormFile>? ArchivosNuevos { get; set; } = new List<IFormFile>();

        public List<int>? ArchivosParaEliminar { get; set; } = new List<int>();
    }
}
