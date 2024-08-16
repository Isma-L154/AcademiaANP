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
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly AnpdesarrolloContext _dbContext;
        private readonly SignInManager<Usuario> _signInManager;

        public UsuarioController(UserManager<Usuario> userManager, AnpdesarrolloContext dbContext, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Usuario model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var oldUserName = user.UserName;

            // Verificar si el nuevo nombre de usuario ya está en uso
            if (model.UserName != oldUserName)
            {
                var existingUser = await _userManager.FindByNameAsync(model.UserName);
                if (existingUser != null)
                {
                    TempData["UsernameInUse"] = "El nombre de usuario ya está en uso. Por favor, elija otro.";
                    return View(model);
                }
            }

            user.Nombre = model.Nombre;
            user.PrimApellido = model.PrimApellido;
            user.SegApellido = model.SegApellido;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;
            user.Notificaciones = model.Notificaciones;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Guardar cambios en el DbContext
                await _dbContext.SaveChangesAsync();

                // Cerrar sesión si el nombre de usuario ha cambiado
                if (oldUserName != user.UserName)
                {
                    await _signInManager.SignOutAsync();
                    TempData["Message"] = "Su nombre de usuario ha sido actualizado. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult CuentaCreada()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
