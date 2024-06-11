using ANP_Academy.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ANP_Academy.ViewModel
{
    public class RoleUpdateViewModel
    {
        public Usuario Usuario { get; set; }
        [Required(ErrorMessage = "The Roles field is required.")]
        public string SelectedRoleId { get; set; }
        public SelectList Roles { get; set; }
        public string Role { get; set; }
    }
}




