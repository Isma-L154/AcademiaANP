using ANP_Academy.DAL.Models;
using ANP_Academy.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ANP_Academy.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AnpdesarrolloContext _dbContext;

        public AdminController(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, AnpdesarrolloContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> GestionRoles()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = new List<RoleUpdateViewModel>();

            foreach (var user in usuarios)
            {
                var userRolesList = await _userManager.GetRolesAsync(user);
                var userRole = userRolesList.FirstOrDefault();

                userRoles.Add(new RoleUpdateViewModel
                {
                    Usuario = user,
                    Role = userRole,
                    Roles = new SelectList(roles, "Id", "Name", roles.FirstOrDefault(r => r.Name == userRole)?.Id)
                });
            }

            return View(userRoles);
        }


        [HttpGet]
        public async Task<IActionResult> EditarRol(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();

            var roleSelectList = new SelectList(roles, "Id", "Name", roles.FirstOrDefault(r => r.Name == userRole)?.Id);

            var model = new RoleUpdateViewModel
            {
                Usuario = user,
                Role = userRole,
                Roles = roleSelectList
            };

            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> ActualizarRol(RoleUpdateViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // Recargar la lista de roles para la vista
        //        var roles = await _roleManager.Roles.ToListAsync();
        //        model.Roles = new SelectList(roles, "Id", "Name", model.SelectedRoleId);
        //        return View("EditarRol", model); // Cambiar a la vista de edición de rol
        //    }

        //    var user = await _userManager.FindByIdAsync(model.Usuario.Id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var currentRoles = await _userManager.GetRolesAsync(user);
        //    var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", "Failed to remove user roles.");
        //        var roles = await _roleManager.Roles.ToListAsync();
        //        model.Roles = new SelectList(roles, "Id", "Name", model.SelectedRoleId);
        //        return View("EditarRol", model); // Cambiar a la vista de edición de rol
        //    }

        //    var newRole = await _roleManager.FindByIdAsync(model.SelectedRoleId);
        //    result = await _userManager.AddToRoleAsync(user, newRole.Name);

        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("GestionRoles");
        //    }

        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }

        //    var roleList = await _roleManager.Roles.ToListAsync();
        //    model.Roles = new SelectList(roleList, "Id", "Name", model.SelectedRoleId);
        //    return View("EditarRol", model); // Cambiar a la vista de edición de rol
        //}


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

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(Usuario model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.Nombre = model.Nombre;
            user.PrimApellido = model.PrimApellido;
            user.SegApellido = model.SegApellido;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Guardar cambios en el DbContext
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("GestionUsuarios");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
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
