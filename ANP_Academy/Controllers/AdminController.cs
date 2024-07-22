using ANP_Academy.DAL.Models;
using ANP_Academy.Models;
using ANP_Academy.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ANP_Academy.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AnpdesarrolloContext _dbContext;
        private readonly IConfiguration _configuration;

        public AdminController(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, AnpdesarrolloContext dbContext, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _configuration = configuration;
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

        public async Task<IActionResult> MostrarFacturas(string filtro, string valor)
        {
            var facturas = from f in _dbContext.Facturas
                           .Include(f => f.Usuario)
                           .Include(f => f.Suscripcion)
                           .Include(f => f.Solicitud)
                           select f;

            return View(await facturas.ToListAsync());
        }

        public async Task<IActionResult> VerDetalleFactura(int id)
        {
            var factura = await _dbContext.Facturas
                .Include(f => f.Usuario)
                .Include(f => f.Suscripcion)
                .Include(f => f.Solicitud)
                .FirstOrDefaultAsync(f => f.IdFactura == id);

            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
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

        public async Task<IActionResult> EstadoSuscripciones()
        {
            var solicitudes = await _dbContext.Solicitudes
                .Include(s => s.User)
                .Include(s => s.SuscripcionEntity)
                .ToListAsync();

            return View(solicitudes);
        }


        public async Task<IActionResult> VerComprobante(int id)
        {
            var solicitud = await _dbContext.Solicitudes.FindAsync(id);
            if (solicitud == null || solicitud.Comprobante == null)
            {
                return NotFound();
            }

            return File(solicitud.Comprobante, "image/jpeg"); 
        }


        public async Task<IActionResult> VerImagen(int id)
        {
            var solicitud = await _dbContext.Solicitudes
                .Include(s => s.User)
                .Include(s => s.SuscripcionEntity)
                .FirstOrDefaultAsync(m => m.IdSolicitud == id);
            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }

        public async Task<IActionResult> ControlSuscrip()
        {
            var solicitudes = await _dbContext.Solicitudes
                .Include(s => s.User)
                .Include(s => s.SuscripcionEntity)
                .Where(s => s.FechaInicio == null && s.FechaFinal == null && s.Estado == null)
                .Select(s => new SolicitudesViewModel
                {
                    IdSolicitud = s.IdSolicitud,
                    UserId = s.User.Id,
                    IdSuscripcion = s.IdSuscripcion,
                    NombreSuscripcion = s.SuscripcionEntity.Nombre,
                    Comprobante = s.Comprobante,
                    FechaSolicitud = s.FechaSolicitud
                }).ToListAsync();

            return View(solicitudes);
        }

        [HttpPost]
        public async Task<IActionResult> AprobarSolicitud(int idSolicitud, string userId, int idSuscripcion)
        {
            var solicitud = await _dbContext.Solicitudes.FindAsync(idSolicitud);
            if (solicitud == null)
            {
                return NotFound();
            }

            var suscripcion = await _dbContext.Suscripciones.FindAsync(idSuscripcion);
            if (suscripcion == null)
            {
                return NotFound();
            }

            // Establece fechas de inicio y finalización
            solicitud.FechaInicio = DateTime.Now;
            solicitud.FechaFinal = DateTime.Now.AddMonths(suscripcion.Duracion);

            // Actualizar el estado
            solicitud.Estado = true;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Remover roles actuales
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Agregar nuevo rol
            var result = await _userManager.AddToRoleAsync(user, "Estudiante");
            if (!result.Succeeded)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            _dbContext.Update(solicitud);
            await _dbContext.SaveChangesAsync();

            var factura = new Factura
            {
                Fecha = DateTime.Now,
                Precio = suscripcion.Precio,
                IdUser = userId,
                IdSuscripcion = idSuscripcion,
                IdSolicitud = idSolicitud
            };

            _dbContext.Facturas.Add(factura);
            await _dbContext.SaveChangesAsync();

            // Enviar correo electrónico 
            string destinatario = user.Email;  
            string asunto = "Solicitud Aprobada";
            string mensaje = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333333;
        }}
       
        .content {{
            padding: 20px;
        }}
        .footer {{
            margin-top: 20px;
            padding: 10px;
            text-align: center;
            font-size: 12px;
            color: #777777;
        }}
        .btn {{
            display: inline-block;
            padding: 10px 20px;
            font-size: 16px;
            color: white;
            background-color: #07AA20;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 20px;
        }}
        .btn:hover {{
            background-color: #0056b3;
        }}
    </style>
</head>
<body>

    <div class='content'>
        <h1>Estimado(a), {user.Nombre} {user.PrimApellido} {user.SegApellido},</h1>
        <p>Tu suscripción a la <strong>Academia Nacional de Parrilleros Rodrigo Morales</strong> ha sido aprobada de manera exitosa.</p>
        <p>Tu suscripción estará activa hasta el día: <strong>{solicitud.FechaFinal:dd/MM/yyyy}</strong>.</p>
        <p>Gracias por ser parte de <strong>ANP Academy</strong>.</p>
        <a href='https://www.youtube.com/' class='btn'>Visita nuestro sitio web</a>
    </div>

    <div class='footer'>
        <p>ANP Academy - Todos los derechos reservados.</p>
    </div>
</body>
</html>
";


            try
            {
                SendEmail(destinatario, asunto, mensaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }

            return RedirectToAction("ControlSuscrip");
        }

        [HttpPost]
        public async Task<IActionResult> RechazarSolicitud(int idSolicitud)
        {
            var solicitud = await _dbContext.Solicitudes.FindAsync(idSolicitud);
            if (solicitud == null)
            {
                return NotFound();
            }

            solicitud.Estado = false;
            _dbContext.Update(solicitud);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ControlSuscrip");
        }

        public void SendEmail(string destinatario, string asunto, string mensaje)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            string CuentaEmail = emailSettings["CuentaEmail"];
            string PasswordEmail = emailSettings["PasswordEmail"];
            string Host = emailSettings["Host"];
            int Port = int.Parse(emailSettings["Port"]);

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(destinatario));
            msg.From = new MailAddress(CuentaEmail);
            msg.Subject = asunto;
            msg.Body = mensaje;
            msg.IsBodyHtml = true;  

            SmtpClient client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(CuentaEmail, PasswordEmail),
                Port = Port,
                Host = Host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            client.Send(msg);
        }

    }
}
