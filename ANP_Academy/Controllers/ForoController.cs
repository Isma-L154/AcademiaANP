using ANP_Academy.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ANP_Academy.Controllers
{
    public class ForoController : Controller
    {

        private readonly AnpdesarrolloContext _context;

        public ForoController(AnpdesarrolloContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var anpdesarrolloContext = _context.Publicaciones.Include(p => p.IdComentarioNavigation);
            return View(await anpdesarrolloContext.ToListAsync());
        }


        public IActionResult MisPublicaciones()
        {
            return View();
        }


        // GET: Publicaciones/Create
        [HttpGet]
        [Authorize]
        public IActionResult CrearPublicacion()
        {
            return View();
        }

        // POST: Publicaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CrearPublicacion([Bind("IdPublicacion,Descripcion,FechaPublicacion,IdComentario,IdUser")] Publicacion publicacion)
        {
            var identidad = User.Identity as ClaimsIdentity;
            string idUsuarioLoggeado = identidad.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            publicacion.CodigoUsuarioId = idUsuarioLoggeado;

            if (ModelState.IsValid)
            {
                publicacion.FechaPublicacion = DateOnly.FromDateTime(DateTime.Now); // Obtener la fecha actual

                _context.Add(publicacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publicacion);
        }



        public IActionResult EditarPublicacion() 
        {
            return View();
        }
    }
}
