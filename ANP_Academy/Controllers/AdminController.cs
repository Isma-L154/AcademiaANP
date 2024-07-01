using ANP_Academy.DAL.Models;
using ANP_Academy.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
                var role = roles.FirstOrDefault(); 

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

        public async Task<IActionResult> GestionInventario()
        {
            var inventarios = await _dbContext.Inventarios
                .Include(i => i.IdCategoriaNavigation)
                .Include(i => i.IdProveedorNavigation)
                .Include(i => i.IdUbicacionNavigation)
                .ToListAsync();
            return View(inventarios);
        }

        public async Task<IActionResult> CrearInventario()
        {
            ViewBag.Categorias = await _dbContext.Categorias.ToListAsync();
            ViewBag.Proveedores = await _dbContext.Proveedores.ToListAsync();
            ViewBag.Ubicaciones = await _dbContext.Ubicacions.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearInventario(Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(inventario);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(GestionInventario));
            }
            return View(inventario);
        }

        public async Task<IActionResult> EditarInventario(int id)
        {
            var inventario = await _dbContext.Inventarios
                .Include(i => i.IdCategoriaNavigation)
                .Include(i => i.IdProveedorNavigation)
                .Include(i => i.IdUbicacionNavigation)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventario == null)
            {
                return NotFound();
            }

            ViewBag.Categorias = await _dbContext.Categorias.ToListAsync();
            ViewBag.Proveedores = await _dbContext.Proveedores.ToListAsync();
            ViewBag.Ubicaciones = await _dbContext.Ubicacions.ToListAsync();

            return View(inventario);
        }

        [HttpPost]
        public async Task<IActionResult> EditarInventario(Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(inventario);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(GestionInventario));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_dbContext.Inventarios.Any(e => e.Id == inventario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Categorias = await _dbContext.Categorias.ToListAsync();
            ViewBag.Proveedores = await _dbContext.Proveedores.ToListAsync();
            ViewBag.Ubicaciones = await _dbContext.Ubicacions.ToListAsync();
            return View(inventario);
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

        public IActionResult MostrarContabilidad()
        {
            return View();
        }

        public IActionResult MostrarFacturas()
        {
            return View();
        }

        public async Task<IActionResult> GestionPlanes()
        {
            return View(await _dbContext.Suscripciones.ToListAsync());
        }


        // GET: Suscripciones/Details/5
        public async Task<IActionResult> DetailsPlanes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _dbContext.Suscripciones
                .FirstOrDefaultAsync(m => m.IdSuscripcion == id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            return View(suscripcion);
        }


        // GET: Suscripciones/Create
        public IActionResult CreatePlanes()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlanes([Bind("IdSuscripcion,Nombre,Precio,Duracion")] Suscripcion suscripcion)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(suscripcion);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(GestionPlanes));
            }
            return View(suscripcion);
        }

        // GET: Suscripciones/Edit/5
        public async Task<IActionResult> EditPlanes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _dbContext.Suscripciones.FindAsync(id);
            if (suscripcion == null)
            {
                return NotFound();
            }
            return View(suscripcion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPlanes(int id, [Bind("IdSuscripcion,Nombre,Precio,Duracion")] Suscripcion suscripcion)
        {
            if (id != suscripcion.IdSuscripcion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(suscripcion);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuscripcionExists(suscripcion.IdSuscripcion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GestionPlanes));
            }
            return View(suscripcion);
        }


        // GET: Suscripciones/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeletePlanes(int id)
        {
            var suscripcion = await _dbContext.Suscripciones.FindAsync(id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            suscripcion.IsDeleted = true; // Marcar como eliminado lógicamente
            _dbContext.Suscripciones.Update(suscripcion);
            var result = await _dbContext.SaveChangesAsync();

            if (result > 0)
            {
                return RedirectToAction("GestionPlanes");
            }

            ModelState.AddModelError(string.Empty, "Error al eliminar la suscripción");
            return RedirectToAction("GestionPlanes");
        }

        // POST: Suscripciones/Reactivate
        [HttpPost]
        public async Task<IActionResult> ReactivatePlanes(int id)
        {
            var suscripcion = await _dbContext.Suscripciones.FindAsync(id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            suscripcion.IsDeleted = false; // Reactivar la suscripción
            _dbContext.Suscripciones.Update(suscripcion);
            var result = await _dbContext.SaveChangesAsync();

            if (result > 0)
            {
                return RedirectToAction("GestionPlanes");
            }

            ModelState.AddModelError(string.Empty, "Error al reactivar la suscripción");
            return RedirectToAction("GestionPlanes");
        }



        private bool SuscripcionExists(int id)
        {
            return _dbContext.Suscripciones.Any(e => e.IdSuscripcion == id);
        }

        public IActionResult MisSuscripciones()
        {
            return View();
        }

    }
}
