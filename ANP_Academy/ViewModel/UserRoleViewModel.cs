using ANP_Academy.DAL.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ANP_Academy.ViewModel
{
    public class UserRoleViewModel
    {
        public Usuario Usuario { get; set; }
        public string Role { get; set; }
        public SelectList Roles { get; set; }
    }
}

