using ANP_Academy.DAL.Migrations.Anpdesarrollo;
using ANP_Academy.DAL.Models;
using ANP_Academy.ViewModel;
using ANP_Academy.ViewModel.Foro;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
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
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            //Esto se hace para la paginacion
            var publicaciones = await _context.Publicaciones
                    .Include(p => p.CodigoUsuario)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            var totalPublicaciones = await _context.Publicaciones.CountAsync();
            var totalPages = (int)Math.Ceiling(totalPublicaciones / (double)pageSize);

            var comentarios = await _context.Comentarios.Include(c => c.CodigoUsuario).ToListAsync();
            var ComentarioPubli = await _context.PublicacionComentarios.ToListAsync();

            var viewModel = new PublicacionComentarioViewModel
            {
                Publicaciones = publicaciones,
                Comentarios = comentarios,
                ComentariosPubli = ComentarioPubli,
                PageNumber = pageNumber,
                TotalPages = totalPages
            };

            return View(viewModel);
        }


        [Authorize]
        public async Task<IActionResult> MisPublicaciones(int pageNumber = 1, int pageSize = 10)
        {

            var publicaciones = await _context.Publicaciones
                     .Include(p => p.CodigoUsuario)
                     .Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();
            var totalPublicaciones = await _context.Publicaciones.CountAsync();
            var totalPages = (int)Math.Ceiling(totalPublicaciones / (double)pageSize);

            var identidadUsuario = User.Identity as ClaimsIdentity;
            string userId = identidadUsuario.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            ViewData["UserId"] = userId;

            var viewModel = new MisPublicacionesViewModel
            {
                Publicaciones = publicaciones,
                PageNumber = pageNumber,
                TotalPages = totalPages
            };

            return View(viewModel);
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
        public async Task<IActionResult> CrearPublicacion( Publicacion publicacion , IFormFile ArchivoMultimedia)
        {
            string base64String = null;
            string MultimediaType = null;
            long MultimediaSize = 0;
            string MultimediaName = null;

            if (ModelState.IsValid)
            {
                var identidad = User.Identity as ClaimsIdentity;
                string idUsuarioLoggeado = identidad.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                if (idUsuarioLoggeado == null)
                {
                    return Unauthorized();
                }
                
                if (ArchivoMultimedia != null && ArchivoMultimedia.Length > 0) {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ArchivoMultimedia.CopyToAsync(memoryStream);
                        var fileBytes = memoryStream.ToArray();
                        base64String = Convert.ToBase64String(fileBytes);
                    }
                    MultimediaType = ArchivoMultimedia.ContentType;
                    MultimediaSize = ArchivoMultimedia.Length;
                    MultimediaName = ArchivoMultimedia.FileName;
                }

                publicacion.CodigoUsuarioId = idUsuarioLoggeado;
                publicacion.FechaPublicacion = DateOnly.FromDateTime(DateTime.Now);
                publicacion.MultimediaData = base64String;
                publicacion.MultimediaType = MultimediaType;
                publicacion.MultimediaSize = MultimediaSize;
                publicacion.MultimediaName = MultimediaName;

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
            //Aca se seleccionan los comentarios relacionados a la publicacion para que tambien se eliminen de la tabla
            var comentariosRelacionados = _context.PublicacionComentarios
                    .Where(pc => pc.PublicacionId == IdPublicacion)
                    .Select(pc => pc.ComentarioId)
                    .ToList();

            var comentarios = _context.Comentarios
                .Where(c => comentariosRelacionados.Contains(c.IdComentario))
                .ToList(); 
            
            if (publicacion != null)
            {
                _context.Comentarios.RemoveRange(comentarios);
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
        public async Task<IActionResult> Editar(int IdPublicacion, [Bind("IdPublicacion,Descripcion,Titulo")] Publicacion publicacion)
        {
            if (IdPublicacion != publicacion.IdPublicacion)
            {
                return NotFound();
            }
            var PubliOriginal = await _context.Publicaciones.FindAsync(IdPublicacion);
            PubliOriginal.Titulo = publicacion.Titulo;
            PubliOriginal.Descripcion = publicacion.Descripcion;


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(PubliOriginal);
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

        //POST para ingresar un reporte de una publicacion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReporte([Bind("ReporteId,Motivo,Explicacion,CodigoUsuarioId,PublicacionId")] PublicacionesReportadas publicacionesReportadas, int IdPublicacion)
        {
            var identidad = User.Identity as ClaimsIdentity;
            string idUsuarioLoggeado = identidad.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (idUsuarioLoggeado == null)
            {
                return Unauthorized();
            }
            publicacionesReportadas.CodigoUsuarioId = idUsuarioLoggeado;
            publicacionesReportadas.PublicacionId = IdPublicacion;

            if (ModelState.IsValid)
            {
                _context.Add(publicacionesReportadas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publicacionesReportadas);
        }
    }
}
