using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANP_Academy.DAL.Models{
    [Table("Users")]
    public class Usuario : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string PrimApellido { get; set; }
        public string SegApellido { get; set; }

        public bool? Activo { get; set; }

        public bool? Suscrito { get; private set; }

        public int? SuscripcionId { get; set; }
        public Suscripcion? Suscripcion { get; set; }

        public virtual ICollection<Solicitudes> Solicitudes { get; set; } = new List<Solicitudes>();


    }
}
