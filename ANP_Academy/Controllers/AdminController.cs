﻿using ANP_Academy.DAL.Models;
using ANP_Academy.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace ANP_Academy.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly AnpdesarrolloContext _dbContext;
        public AdminController(
            UserManager<Usuario> userManager,
            RoleManager<IdentityRole> roleManager,
            AnpdesarrolloContext dbContext)
        {
            _userManager = userManager;
            this._dbContext = dbContext;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> DesactivarUsuario(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Activo = false;
            var result = await _userManager.UpdateAsync(usuario);

            if (result.Succeeded)
            {
                return RedirectToAction("GestionUsuarios");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("GestionUsuarios");
        }

        [HttpPost]
        public async Task<IActionResult> ActivarUsuario(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Activo = true;
            var result = await _userManager.UpdateAsync(usuario);

            if (result.Succeeded)
            {
                return RedirectToAction("GestionUsuarios");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("GestionUsuarios");
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
            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault(); // Assuming a user has only one role

                userRoles.Add(new UserRoleViewModel
                {
                    Usuario = user,
                    Role = role
                });
            }

            return View(userRoles);
        }



        public IActionResult EditarUsuario()
        {
            return View();
        }

        public IActionResult CrearUsuario()
        {
            return View();
        }

        public async Task<IActionResult> GestionInventario()
        {
            var inventarios = await _dbContext.Inventarios.ToListAsync();
            return View(inventarios); 
        }

        public IActionResult CrearInventario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearInventario(Inventario inventario)
        {
            
                _dbContext.Add(inventario);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(GestionInventario));
            
        }

        public IActionResult EliminarInventario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EliminarInventario(int id)
        {
            var inventario = await _dbContext.Inventarios.FindAsync(id);

            if (inventario == null)
            {
                return NotFound();
            }

            _dbContext.Inventarios.Remove(inventario);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(GestionInventario));
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
