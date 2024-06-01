using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaANP.DAL.Models
{
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

        public int? Activo { get; set; }
    }
}
