using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANP_Academy.DAL.Models
{
    [Table("Receta")]
    public class Receta
    {
        [Key]
        public int IdReceta { get; set; }
        [Required]
        public string Titulo { get; set; } = null!;
        [Required]
        public string Descripcion { get; set; } = null!;
        [Required]
        public byte[] Imagen { get; set; } = null!;
        [Required]
        public string URLVideo { get; set; } = null!;
        [Required]
        public ICollection<RecetaArchivo> Archivos { get; set; } = new List<RecetaArchivo>();

    }
}
