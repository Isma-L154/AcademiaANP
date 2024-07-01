using ANP_Academy.DAL.Models;
using ANP_Academy.ViewModel;
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
            var publicaciones = await _context.Publicaciones.Include(p => p.CodigoUsuario).ToListAsync();
            var comentarios = await _context.Comentarios.ToListAsync();
            var ComentarioPubli = await _context.PublicacionComentarios.ToListAsync();

            var viewModel = new PublicacionComentarioViewModel
            {
                Publicaciones = publicaciones,
                Comentarios = comentarios,
                ComentariosPubli = ComentarioPubli
            };

            return View(viewModel);
        }


        [Authorize]
        public async Task<IActionResult> MisPublicaciones()
        {
            var anpdesarrolloContext = _context.Publicaciones.Include(p => p.CodigoUsuario);
            var identidadUsuario = User.Identity as ClaimsIdentity;
            string userId = identidadUsuario.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            ViewData["UserId"] = userId;
            return View(await anpdesarrolloContext.ToListAsync());
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


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int IdPublicacion)
        {
            var publicacion = await _context.Publicaciones.FindAsync(IdPublicacion);
            if (publicacion != null)
            {
                _context.Publicaciones.Remove(publicacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MisPublicaciones));
        }



        //GET: EditarPublicaciones/{ID}
        public async Task<IActionResult> EditarPublicacion(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }
            var publicacion = await _context.Publicaciones.FindAsync(id);
            if (publicacion == null)
            {
                return NotFound();
            }
            return View(publicacion);

        }

        //POST: EditarPublicaciones/{ID}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int IdPublicacion, [Bind("IdPublicacion,Descripcion,IdUser")] Publicacion publicacion)
        {
            if (IdPublicacion != publicacion.IdPublicacion)
            {
                return NotFound();
            }
            //TODO Resolve BUG with the date
            var identidad = User.Identity as ClaimsIdentity;
            string idUsuarioLoggeado = identidad.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value; 
            publicacion.CodigoUsuarioId = idUsuarioLoggeado;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publicacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicacionExiste(publicacion.IdPublicacion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MisPublicaciones));
            }
            return RedirectToAction(nameof(MisPublicaciones));
        }

        private bool PublicacionExiste(int id)
        {
            return _context.Publicaciones.Any(e => e.IdPublicacion == id);
        }

        //POST CrearComentario
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CrearComentario([Bind("ContenidoComentario")] Comentario comentario , int IdPublicacion)
        {
            var identidad = User.Identity as ClaimsIdentity;
            string idUsuarioLoggeado = identidad.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (idUsuarioLoggeado == null)
            {
                return Unauthorized();
            }

            comentario.CodigoUsuarioId = idUsuarioLoggeado;
            comentario.FechaComentario  = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(comentario);
                await _context.SaveChangesAsync();
                
                var PublicacionComentario = new PublicacionComentario
                {
                    PublicacionId = IdPublicacion,
                    ComentarioId = comentario.IdComentario
                };

                _context.Add(PublicacionComentario);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
