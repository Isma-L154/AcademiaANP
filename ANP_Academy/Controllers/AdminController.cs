using ANP_Academy.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ANP_Academy.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        public AdminController(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }


        public IActionResult ControlSuscrip()
        {
            return View();
        }
        public IActionResult GestionForo()
        {
            return View();
        }

        public IActionResult GestionContenido()
        {
            return View();
        }

        public IActionResult ContenidoFormulario()
        {
            return View();
        }

        public async Task<IActionResult> GestionUsuarios()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            return View(usuarios);
        }

        public IActionResult EditarUsuario()
        {
            return View();
        }

        public IActionResult CrearUsuario()
        {
            return View();
        }

        public IActionResult GestionInventario()
        {
            return View();
        }

        public IActionResult CrearInventario()
        {
            return View();
        }

        public IActionResult EditarInventario()
        {
            return View();
        }

        public IActionResult MostrarContabilidad()
        {
            return View();
        }

        public IActionResult MostrarFacturas()
        {
            return View();
        }


    }
}
