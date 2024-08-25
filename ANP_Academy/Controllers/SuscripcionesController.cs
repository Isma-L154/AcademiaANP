using Microsoft.AspNetCore.Mvc;
using ANP_Academy.DAL.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using ANP_Academy.ViewModel;
using ANP_Academy.Models;
using System.Diagnostics;


namespace ANP_Academy.Controllers
{
    public class SuscripcionesController : Controller
    {
        private readonly AnpdesarrolloContext _dbContext;
        private readonly UserManager<Usuario> _userManager;

        public SuscripcionesController(AnpdesarrolloContext dbContext, UserManager<Usuario> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // GET: Suscripciones
        public async Task<IActionResult> Index()
        {
            var suscripciones = await _dbContext.Suscripciones
                .Where(s => !s.IsDeleted) // Filtrar las suscripciones no eliminadas
                .ToListAsync();
            return View(suscripciones);
        }

        public async Task<IActionResult> AdquirirSuscripcion(int id)
        {
            var suscripcion = await _dbContext.Suscripciones
                .FirstOrDefaultAsync(s => s.IdSuscripcion == id);

            if (suscripcion == null)
            {
                return NotFound("La suscripción no fue encontrada.");
            }

            return View(suscripcion);
        }

        [HttpPost]
        public async Task<IActionResult> AdjuntarComprobante(int IdSuscripcion, string UserId, IFormFile Comprobante)
        {
            if (Comprobante == null || Comprobante.Length == 0)
            {
                ModelState.AddModelError("", "Debe adjuntar un comprobante.");
                return RedirectToAction("Index");
            }

            byte[] comprobanteData;
            using (var memoryStream = new MemoryStream())
            {
                await Comprobante.CopyToAsync(memoryStream);
                comprobanteData = memoryStream.ToArray();
            }

            var solicitud = new Solicitudes
            {
                Id = UserId,
                IdSuscripcion = IdSuscripcion,
                Comprobante = comprobanteData,
                FechaSolicitud = DateTime.Now
            };

            _dbContext.Solicitudes.Add(solicitud);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> MisSuscripciones()
        {
            string userId = _userManager.GetUserId(User);
            var suscripcionesUsuario = await _dbContext.Solicitudes
                .Include(s => s.SuscripcionEntity)
                .Where(s => s.User.Id == userId)
                .ToListAsync();

            var suscripcionesPendientes = suscripcionesUsuario.Where(s => s.FechaInicio == null && s.FechaFinal == null && s.Estado == null).ToList();
            var SuscripcionesActivas = suscripcionesUsuario.Where(s => s.FechaInicio != null && s.FechaFinal != null && s.Estado == true).ToList();
            var suscripcionesRechazadas = suscripcionesUsuario.Where(s => s.FechaInicio == null && s.FechaFinal == null && s.Estado == false).ToList();

            return View(new MisSuscripcionesViewModel
            {
                SuscripcionesPendientes = suscripcionesPendientes,
                SuscripcionesActivas = SuscripcionesActivas,
                SuscripcionesRechazadas = suscripcionesRechazadas
            });
        }

        [HttpPost]
        public async Task<IActionResult> CancelarSuscripcion(int idSolicitud)
        {
            var solicitud = await _dbContext.Solicitudes.FindAsync(idSolicitud);
            if (solicitud == null)
            {
                return NotFound();
            }

            // Actualizar la solicitud
            solicitud.FechaFinal = DateTime.Now; // Establecer la fecha de finalización al momento actual
            solicitud.Estado = false;           // Marcar la solicitud como rechazada

            // Obtener el usuario asociado a la solicitud
            var user = await _userManager.FindByIdAsync(solicitud.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Remover roles actuales y asignar el rol de Cliente
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var result = await _userManager.AddToRoleAsync(user, "Cliente");
            if (!result.Succeeded)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            // Marcar al usuario como no suscrito
            user.Suscrito = false;
            _dbContext.Update(user);

            // Guardar los cambios en la base de datos
            _dbContext.Update(solicitud);
            await _dbContext.SaveChangesAsync();

            // Redirigir al control de suscripciones
            return RedirectToAction("MisSuscripciones");
        }


    }
}
