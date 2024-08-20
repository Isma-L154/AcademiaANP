using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANP_Academy.DAL.Models
{
    [Table("ClaseVista")]
    public class ClaseVista
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual Usuario Usuario { get; set; }

        public int IdClase { get; set; }

        [ForeignKey("IdClase")]
        public virtual Clase Clase { get; set; }

        public DateTime FechaVista { get; set; }
    }
}