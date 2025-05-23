﻿using ANP_Academy.DAL.Models;
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
using System.Drawing.Printing;
using ANP_Academy.ViewModel.Contabilidad;
using System.Globalization;
using System.Text;
using ANP_Academy.ViewModel.Foro;
using System.Security.Claims;
using ANP_Academy.DAL.Migrations.Anpdesarrollo;
using System.Text.RegularExpressions;

namespace ANP_Academy.Controllers
{
    [Authorize(Roles = "Admin, Profesor")]
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
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(usuario);
            var roles = await _roleManager.Roles.ToListAsync();

            var roleList = new SelectList(roles, "Name", "Name", userRoles.FirstOrDefault());

            ViewBag.Roles = roleList; 

            var viewModel = new UserRoleViewModel
            {
                Usuario = usuario,
                Role = userRoles.FirstOrDefault()
            };

            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> EditarUsuario(Usuario usuario, string role)
        {
            
            var user = await _userManager.FindByIdAsync(usuario.Id);
            if (user == null)
            {
                return NotFound();
            }

            
            user.Nombre = usuario.Nombre;
            user.PrimApellido = usuario.PrimApellido;
            user.SegApellido = usuario.SegApellido;
            user.PhoneNumber = usuario.PhoneNumber;
            user.UserName = usuario.UserName;
            user.Email = usuario.Email;

            if (!Regex.IsMatch(user.PhoneNumber, @"^\d{8}$"))
            {
                ModelState.AddModelError("Usuario.PhoneNumber", "El número de teléfono debe tener exactamente 8 dígitos y solo puede contener números.");

                //Se tiene que pasar un ViewModel por eso esta reventando la validacion y se ocasionaba un bug
                var userRoles = await _userManager.GetRolesAsync(user);
                var roles = await _roleManager.Roles.ToListAsync();
                var roleList = new SelectList(roles, "Name", "Name", userRoles.FirstOrDefault());

                ViewBag.Roles = roleList;

                var viewModel = new UserRoleViewModel
                {
                    Usuario = user, // Pasamos el usuario con los datos actualizados porque de fijo hay que devolver un ViewModel
                    Role = userRoles.FirstOrDefault()
                };

                return View(viewModel); 
            }

            var result = await _userManager.UpdateAsync(user);
            
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(role))
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    foreach (var error in removeResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(usuario);
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, role);
                if (!addRoleResult.Succeeded)
                {
                    foreach (var error in addRoleResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(usuario);
                }
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("GestionUsuarios");
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

        public async Task<IActionResult> MostrarFacturas(string intervalo, string filtro, string valor)
        {
            var suscripciones = await _dbContext.Suscripciones
                .Where(s => !s.IsDeleted) // Excluye las suscripciones eliminadas
                .ToListAsync();

            ViewBag.Suscripciones = suscripciones;

            var facturas = _dbContext.Facturas
                .AsNoTracking()
                .Include(f => f.Usuario)
                .Include(f => f.Suscripcion)
                .Include(f => f.Solicitud)
                .AsQueryable();

            // Filtrar por suscripción
            if (!string.IsNullOrEmpty(intervalo))
            {
                var duracion = suscripciones.FirstOrDefault(s => s.Nombre.Equals(intervalo, StringComparison.OrdinalIgnoreCase))?.Duracion;
                if (duracion.HasValue)
                {
                    facturas = facturas.Where(f => f.Suscripcion.Duracion == duracion.Value);
                }
            }

            // Filtrar por INFO del usuario
            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrEmpty(valor))
            {
                switch (filtro.ToLower())
                {
                    case "identificador":
                        facturas = facturas.Where(f => f.Usuario.Id.ToString() == valor);
                        break;
                    case "nombre":
                        facturas = facturas.Where(f => f.Usuario.Nombre.Contains(valor));
                        break;
                    case "correo electrónico":
                        facturas = facturas.Where(f => f.Usuario.Email.Contains(valor));
                        break;
                    case "fecha":
                        if (DateTime.TryParse(valor, out DateTime fecha))
                        {
                            facturas = facturas.Where(f => f.Fecha.Date == fecha.Date);
                        }
                        break;
                }
            }

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

            
            solicitud.FechaInicio = DateTime.Now;
            solicitud.FechaFinal = DateTime.Now.AddMonths(suscripcion.Duracion);
            solicitud.Estado = true;

            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await _userManager.AddToRoleAsync(user, "Estudiante");
            if (!result.Succeeded)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            
            user.Suscrito = true;

            
            _dbContext.Update(solicitud);
            _dbContext.Update(user);
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
            background-color: #aa2a07;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 20px;
        }}
        .btn:hover {{
            background-color: #0056b3;
        }}
        .factura {{
            margin-top: 20px;
            border-top: 1px solid #cccccc;
            padding-top: 20px;
        }}
        .factura h2 {{
            color: #aa2a07;
        }}
        .factura table {{
            width: 100%;
            border-collapse: collapse;
        }}
        .factura table, .factura th, .factura td {{
            border: 1px solid #cccccc;
        }}
        .factura th, .factura td {{
            padding: 10px;
            text-align: left;
        }}
    </style>
</head>
<body>

    <div class='content'>
        <h1>Estimado(a), {user.Nombre} {user.PrimApellido} {user.SegApellido},</h1>
        <p>Tu suscripción a la <strong>Academia Nacional de Parrilleros Rodrigo Morales</strong> ha sido aprobada de manera exitosa.</p>
        <p>Tu suscripción estará activa hasta el día: <strong>{solicitud.FechaFinal:dd/MM/yyyy}</strong>.</p>
        <p>Gracias por ser parte de <strong>ANP Academy</strong>.</p>
        <a href='https://academianp.azurewebsites.net/' class='btn'>Visita nuestro sitio web</a>

        <div class='factura'>
            <h2>Factura</h2>
            <table>
                <tr>
                    <th>Fecha</th>
                    <th>Precio</th>
                    <th>Usuario</th>
                    <th>Suscripción</th>
                </tr>
                <tr>
                    <td>{factura.Fecha:dd/MM/yyyy}</td>
                    <td>{factura.Precio:C}</td>
                    <td>{user.Nombre} {user.PrimApellido} {user.SegApellido}</td>
                    <td>{suscripcion.Nombre}</td>
                </tr>
            </table>
        </div>
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


        //CONTABILIDAD 
        //TODO Organize this whole controller in different folders, is disorganize af
        public IActionResult MostrarContabilidad()
        {
            var suscripciones = _dbContext.Suscripciones.ToList();
            var facturas = _dbContext.Facturas.ToList();
            var suscripcionesFacturas = suscripciones.Select(suscripcion => new SuscripcionFacturaViewModel
            {
                Nombre = suscripcion.Nombre,
                Precio = suscripcion.Precio,
                Cantidad = facturas.Count(f => f.IdSuscripcion == suscripcion.IdSuscripcion),
                Total = suscripcion.Precio * facturas.Count(f => f.IdSuscripcion == suscripcion.IdSuscripcion),
                FechaFactura = facturas.FirstOrDefault(f => f.IdSuscripcion == suscripcion.IdSuscripcion)?.Fecha.ToString("yyyy-MM") ?? "No Disponible"
            }).ToList();

            var totalGeneral = suscripcionesFacturas.Sum(s => s.Total);

            var viewModel = new ContabilidadViewModel
            {
                SuscripcionesFacturas = suscripcionesFacturas,
                TotalGeneral = totalGeneral
            };

            return View(viewModel);
        }

        public async Task<IActionResult> DescargarFacturas(string fechaCorte)
        {
            if (DateTime.TryParseExact(fechaCorte, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
            {
                var inicioMes = new DateTime(fecha.Year, fecha.Month, 1);
                var finMes = inicioMes.AddMonths(1).AddDays(-1);

                // Asumiendo que las tablas son Facturas, Solicitudes y Suscripciones están relacionadas adecuadamente
                var facturas = await _dbContext.Facturas
                    .Include(f => f.Solicitud)
                    .Include(f => f.Suscripcion)
                    .Where(f => f.Fecha >= inicioMes && f.Fecha <= finMes)
                    .Select(f => new
                    {
                        IdFactura = f.IdFactura,
                        Fecha = f.Fecha,
                        Precio = f.Precio,
                        IdUser = f.IdUser,
                        IdSuscripcion = f.IdSuscripcion,
                        IdSolicitud = f.IdSolicitud
                    })
                    .ToListAsync();

                var sb = new StringBuilder();
                sb.AppendLine("IdFactura,Fecha,Precio,IdUser,IdSuscripcion,IdSolicitud");
                foreach (var factura in facturas)
                {
                    sb.AppendLine($"{factura.IdFactura},{factura.Fecha:yyyy-MM-dd},{factura.Precio},{factura.IdUser},{factura.IdSuscripcion},{factura.IdSolicitud}");
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Facturas.csv");
            }

            return View("Error"); // Retorna a una vista de error si la fecha no es válida
        }


        //FEAATURES PARA GESTION DEL FORO
        public IActionResult GestionForo()
        {
            var PubliReport = _dbContext.PublicacionesReportadas.Include(p => p.CodigoUsuario)
                .Include(p => p.Publicacion)
                .ToList();
            return View(PubliReport);
        }
        public async Task<IActionResult> DetallePublicacion(int id)
        {
            // Obtener la publicación específica por su ID
            var publicacion = await _dbContext.Publicaciones
                .Include(p => p.CodigoUsuario) // Incluye información del usuario
                .FirstOrDefaultAsync(p => p.IdPublicacion == id);

            if (publicacion == null)
            {
                return NotFound();
            }

            var comentarioIds = await _dbContext.PublicacionComentarios
        .Where(pc => pc.PublicacionId == id)
        .Select(pc => pc.ComentarioId)
        .ToListAsync();

            // Obtener los comentarios relacionados usando los IDs
            var comentarios = await _dbContext.Comentarios
                .Include(c => c.CodigoUsuario) 
                .Where(c => comentarioIds.Contains(c.IdComentario)) 
                .ToListAsync();

            // Crear el ViewModel para la vista
            var model = new PublicacionComentarioViewModel
            {
                Publicacion =  publicacion, 
                Comentarios = comentarios
            };

            return View("~/Views/Foro/PubliEspec.cshtml", model);
        }




        [HttpPost]
        public async Task<IActionResult> ConservarPublicacion(int id)
        {
            var PubliReport = await _dbContext.PublicacionesReportadas.FindAsync(id);

            if (PubliReport == null)
            {
                return NotFound();
            }

            _dbContext.PublicacionesReportadas.Remove(PubliReport);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(GestionForo));
        }

        [HttpPost]
        public async Task<IActionResult> EliminarPublicacion(int ReportId, int PubliId)
        {
            var PubliReport = await _dbContext.PublicacionesReportadas.FindAsync(ReportId);
            var PubliForo = await _dbContext.Publicaciones.FindAsync(PubliId);
            
            if (PubliReport == null || PubliForo == null)
            {
                return NotFound();
            }

            _dbContext.Publicaciones.Remove(PubliForo);
            _dbContext.PublicacionesReportadas.Remove(PubliReport);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(GestionForo));
        }
    }
}
