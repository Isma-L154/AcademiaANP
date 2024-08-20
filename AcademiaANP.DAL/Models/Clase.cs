using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANP_Academy.DAL.Models
{
    [Table("Clase")]
    public class Clase
    {
        [Key]
        public int IdClase { get; set; }

        [Required]
        public string Titulo { get; set; } = null!;

        [Required]
        public string Descripcion { get; set; } = null!;

        [Required]
        public byte[] Imagen { get; set; } = null!;

        [Required]
        public string URLVideo { get; set; } = null!;

        [Range(0, 5)]
        public float Rating { get; set; } = 0f;

        public ICollection<ClaseRating> Ratings { get; set; }

        public ICollection<ClaseVista> ClasesVistas { get; set; }
    }
}
