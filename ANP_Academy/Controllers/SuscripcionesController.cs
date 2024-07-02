using Microsoft.AspNetCore.Mvc;
using ANP_Academy.DAL.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using ANP_Academy.ViewModel;


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
            var suscripcionesAprobadas = suscripcionesUsuario.Where(s => s.FechaInicio != null && s.FechaFinal != null && s.Estado == true).ToList();
            var suscripcionesRechazadas = suscripcionesUsuario.Where(s => s.FechaInicio == null && s.FechaFinal == null && s.Estado == false).ToList();

            return View(new MisSuscripcionesViewModel
            {
                SuscripcionesPendientes = suscripcionesPendientes,
                SuscripcionesAprobadas = suscripcionesAprobadas,
                SuscripcionesRechazadas = suscripcionesRechazadas
            });
        }

    }
}
