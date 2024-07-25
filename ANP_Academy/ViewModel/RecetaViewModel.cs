using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ANP_Academy.ViewModel
{
    public class RecetaViewModel
    {
        public int IdReceta { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = null!;

        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = null!;

        [Display(Name = "Imagen")]
        public IFormFile? Imagen { get; set; } // No obligatorio

        [Required]
        [Display(Name = "URL del Video")]
        public string URLVideo { get; set; } = null!;

        [Display(Name = "Archivos Nuevos")]
        public List<IFormFile>? ArchivosNuevos { get; set; } = new List<IFormFile>(); // Nuevos archivos opcionales

        public List<RecetaArchivoViewModel>? ArchivosExistentes { get; set; } = new List<RecetaArchivoViewModel>(); // Archivos existentes

        public List<int>? ArchivosParaEliminar { get; set; } = new List<int>(); // Archivos para eliminar
    }
}
