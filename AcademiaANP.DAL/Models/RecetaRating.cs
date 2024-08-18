using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANP_Academy.DAL.Models
{
    [Table("RecetaRating")]
    public class RecetaRating
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Receta")]
        public int IdReceta { get; set; }

        [ForeignKey("Usuario")]
        public string UserId { get; set; } = null!;

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime Fecha { get; set; }

        public virtual Receta Receta { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
