using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANP_Academy.DAL.Models
{
    [Table("RecetaVista")]
    public class RecetaVista
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual Usuario Usuario { get; set; }

        public int IdReceta { get; set; }

        [ForeignKey("IdReceta")]
        public virtual Receta Receta { get; set; }

        public DateTime FechaVista { get; set; }
    }
}
