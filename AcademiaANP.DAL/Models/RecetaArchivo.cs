using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANP_Academy.DAL.Models
{
    [Table("RecetaArchivo")]
    public class RecetaArchivo
    {
        [Key]
        public int IdArchivo { get; set; }

        [Required]
        public byte[] Archivo { get; set; } = null!;

        [Required]
        public int RecetaId { get; set; }

        [ForeignKey("RecetaId")]
        public Receta Receta { get; set; } = null!;

        [Required]
        public string NombreArchivo { get; set; } = null!;

        [Required]
        public string TipoArchivo { get; set; } = null!;
    }
}
