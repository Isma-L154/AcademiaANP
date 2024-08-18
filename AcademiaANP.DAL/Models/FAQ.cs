using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANP_Academy.DAL.Models
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "La pregunta no puede exceder los 500 caracteres")]
        public string Pregunta { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "La respuesta no puede exceder los 1000 caracteres")]
        public string Respuesta { get; set; }
    }
}
