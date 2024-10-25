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
        [Required(ErrorMessage = "Se requiere el completar el campo Nombre")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Se requiere el completar el campo Primer Apellido")]
        [MaxLength(100)]
        public string PrimApellido { get; set; }

        [Required(ErrorMessage = "Se requiere el completar el campo Segundo Apellido")]
        public string SegApellido { get; set; }

        public bool? Activo { get; set; }

        public bool? Suscrito { get;  set; }

        public int? SuscripcionId { get; set; }
        public Suscripcion? Suscripcion { get; set; }

        public bool Notificaciones { get; set; }

        [StringLength(8, MinimumLength = 8, ErrorMessage = "El número de teléfono debe tener exactamente 8 números.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de teléfono solo debe contener números (0-9).")]
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Solicitudes> Solicitudes { get; set; } = new List<Solicitudes>();
        public virtual ICollection<Notificacion> Notificacion { get; set; } = new List<Notificacion>();
        public virtual ICollection<ClaseVista> ClasesVistas { get; set; } = new List<ClaseVista>();

    }
}
