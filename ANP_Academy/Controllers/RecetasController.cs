using ANP_Academy.DAL.Models;
using ANP_Academy.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Net.Mail;
using System.Net;
using System.Numerics;

namespace ANP_Academy.Controllers
{
    
    public class RecetasController : Controller
    {
        private readonly AnpdesarrolloContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly UserManager<Usuario> _userManager;

        public RecetasController(AnpdesarrolloContext dbContext, UserManager<Usuario> userManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _configuration = configuration;
        }
        
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recetas = await _dbContext.Recetas
                .Include(r => r.RecetasVistas)
                .Include(r => r.Ratings)
                .ToListAsync();

            var viewModel = recetas.Select(r => new RecetaViewModel
            {
                IdReceta = r.IdReceta,
                Titulo = r.Titulo,
                Descripcion = r.Descripcion,
                URLVideo = r.URLVideo,
                Rating = r.Ratings.Any() ? (float)r.Ratings.Average(r => r.Rating) : 0,
                EsLeida = r.RecetasVistas.Any(rv => rv.UserId == userId)
            }).ToList();

            return View(viewModel);
        }


        public async Task<IActionResult> InfoReceta(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _dbContext.Recetas
                .Include(r => r.Archivos)
                .FirstOrDefaultAsync(m => m.IdReceta == id);
            if (receta == null)
            {
                return NotFound();
            }
            return View(receta);
        }



        [Authorize(Roles = "Admin, Profesor")]
        public async Task<IActionResult> GestionRecetas()
        {
            var recetas = await _dbContext.Recetas
                .Include(r => r.Archivos)
                .ToListAsync();
            return View(recetas);
        }

        [Authorize(Roles = "Admin, Profesor")]
        public IActionResult CreateRecetas()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecetas(RecetaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validar el tamaño de las imágenes
                if (model.Imagen != null && model.Imagen.Length > 2 * 1024 * 1024) // 2MB
                {
                    ModelState.AddModelError("Imagen", "La imagen no puede ser mayor a 2MB.");
                    return View(model);
                }

                // Validar el tamaño de los archivos
                if (model.ArchivosNuevos != null)
                {
                    foreach (var archivo in model.ArchivosNuevos)
                    {
                        if (archivo.Length > 10 * 1024 * 1024) // 10MB
                        {
                            ModelState.AddModelError("ArchivosNuevos", "Uno de los archivos excede el tamaño máximo de 10MB.");
                            return View(model);
                        }
                    }
                }

                var receta = new Receta
                {
                    Titulo = model.Titulo,
                    Descripcion = model.Descripcion,
                    URLVideo = model.URLVideo,
                    Imagen = model.Imagen != null ? await ConvertToByteArray(model.Imagen) : null
                };

                receta.Archivos = new List<RecetaArchivo>();

                if (model.ArchivosNuevos != null)
                {
                    foreach (var archivo in model.ArchivosNuevos)
                    {
                        var recetaArchivo = new RecetaArchivo
                        {
                            Archivo = await ConvertToByteArray(archivo),
                            NombreArchivo = archivo.FileName,
                            TipoArchivo = archivo.ContentType
                        };
                        receta.Archivos.Add(recetaArchivo);
                    }
                }

                _dbContext.Recetas.Add(receta);
                await _dbContext.SaveChangesAsync();

                // Generar notificación dentro de la plataforma a los usuarios.
                await GenerarNotificacionReceta(receta);
                await NotificarEstudiantesSuscritos(receta);

                return RedirectToAction(nameof(GestionRecetas));
            }

            return View(model);
        }

        private async Task GenerarNotificacionReceta(Receta receta)
        {
            var estudiantesSuscritos = await _userManager.Users
                .Where(u => u.Suscrito == true && u.Notificaciones == true)
                .ToListAsync();

            foreach (var estudiante in estudiantesSuscritos)
            {
                var notificacion = new Notificacion
                {
                    IdUser = estudiante.Id,
                    Contenido = $"¡La Academia ha publicado una nueva receta! Conoce más sobre {receta.Titulo} en la sección de recetas.",
                    IdRecurso = receta.IdReceta,
                    TipoRecurso = "Receta",
                    EsLeido = false,
                    Fecha = DateTime.Now
                };

                _dbContext.Add(notificacion);
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task NotificarEstudiantesSuscritos(Receta receta)
        {
            // Obtén los estudiantes suscritos que desean recibir notificaciones
            var estudiantesSuscritos = await _userManager.Users
                .Where(u => u.Suscrito == true && u.Notificaciones == true)
                .ToListAsync();

            if (estudiantesSuscritos.Count == 0)
            {
                Console.WriteLine("No se encontraron estudiantes suscritos que deseen notificaciones.");
                return;
            }

            foreach (var estudiante in estudiantesSuscritos)
            {
                string destinatario = estudiante.Email;
                string asunto = "Nueva Receta Agregada";
                string mensaje = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                /* estilos CSS */
            </style>
        </head>
        <body>
            <div class='content'>
                <h1>Estimado(a) {estudiante.Nombre} {estudiante.PrimApellido} {estudiante.SegApellido},</h1>
                <p>Se ha agregado una nueva receta titulada <strong>{receta.Titulo}</strong> en <strong>ANP Academy</strong>.</p>
                <p>Descripción: {receta.Descripcion}</p>
                <a href='{receta.URLVideo}' class='btn'>Ver Receta</a>
            </div>
            <div class='footer'>
                <p>ANP Academy - Todos los derechos reservados.</p>
            </div>
        </body>
        </html>";

                try
                {
                    SendEmail(destinatario, asunto, mensaje);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al enviar el correo: " + ex.Message);
                }
            }
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

        [Authorize(Roles = "Admin, Profesor")]
        public async Task<IActionResult> DetailsRecetas(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _dbContext.Recetas
                .Include(r => r.Archivos)
                .FirstOrDefaultAsync(m => m.IdReceta == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        [Authorize(Roles = "Admin, Profesor")]
        public async Task<IActionResult> EditRecetas(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _dbContext.Recetas
                .Include(r => r.Archivos)
                .FirstOrDefaultAsync(r => r.IdReceta == id);
            if (receta == null)
            {
                return NotFound();
            }

            var model = new RecetaViewModel
            {
                IdReceta = receta.IdReceta,
                Titulo = receta.Titulo,
                Descripcion = receta.Descripcion,
                URLVideo = receta.URLVideo,
                ArchivosExistentes = receta.Archivos.Select(a => new RecetaArchivoViewModel
                {
                    IdArchivo = a.IdArchivo,
                    NombreArchivo = a.NombreArchivo
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecetas(int id, RecetaViewModel model)
        {
            if (id != model.IdReceta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Validar el tamaño de las imágenes
                if (model.Imagen != null && model.Imagen.Length > 2 * 1024 * 1024) // 2MB
                {
                    ModelState.AddModelError("Imagen", "La imagen no puede ser mayor a 2MB.");
                    return View(model);
                }

                // Validar el tamaño de los archivos
                if (model.ArchivosNuevos != null)
                {
                    foreach (var archivo in model.ArchivosNuevos)
                    {
                        if (archivo.Length > 10 * 1024 * 1024) // 10MB
                        {
                            ModelState.AddModelError("ArchivosNuevos", "Uno de los archivos excede el tamaño máximo de 10MB.");
                            return View(model);
                        }
                    }
                }

                try
                {
                    var receta = await _dbContext.Recetas
                        .Include(r => r.Archivos)
                        .FirstOrDefaultAsync(r => r.IdReceta == id);
                    if (receta == null)
                    {
                        return NotFound();
                    }

                    receta.Titulo = model.Titulo;
                    receta.Descripcion = model.Descripcion;
                    receta.URLVideo = model.URLVideo;

                    // Actualizar imagen si se cargó una nueva
                    if (model.Imagen != null)
                    {
                        receta.Imagen = await ConvertToByteArray(model.Imagen);
                    }

                    // Eliminar los archivos seleccionados para eliminación
                    if (model.ArchivosParaEliminar != null && model.ArchivosParaEliminar.Any())
                    {
                        var archivosParaEliminar = receta.Archivos
                            .Where(a => model.ArchivosParaEliminar.Contains(a.IdArchivo)).ToList();

                        foreach (var archivo in archivosParaEliminar)
                        {
                            receta.Archivos.Remove(archivo);
                            _dbContext.RecetaArchivos.Remove(archivo);
                        }
                    }

                    // Agregar los nuevos archivos
                    if (model.ArchivosNuevos != null && model.ArchivosNuevos.Any())
                    {
                        foreach (var archivo in model.ArchivosNuevos)
                        {
                            var recetaArchivo = new RecetaArchivo
                            {
                                Archivo = await ConvertToByteArray(archivo),
                                NombreArchivo = archivo.FileName,
                                TipoArchivo = archivo.ContentType
                            };
                            receta.Archivos.Add(recetaArchivo);
                        }
                    }

                    _dbContext.Update(receta);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecetaExists(model.IdReceta))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GestionRecetas));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Profesor")]
        public async Task<IActionResult> DeleteRecetas(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _dbContext.Recetas
                .Include(r => r.Archivos)
                .FirstOrDefaultAsync(m => m.IdReceta == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        [HttpPost, ActionName("DeleteRecetas")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecetasConfirmed(int id)
        {
            var receta = await _dbContext.Recetas
                .Include(r => r.Archivos)
                .FirstOrDefaultAsync(m => m.IdReceta == id);
            _dbContext.Recetas.Remove(receta);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(GestionRecetas));
        }

        private bool RecetaExists(int id)
        {
            return _dbContext.Recetas.Any(e => e.IdReceta == id);
        }

        private async Task<byte[]> ConvertToByteArray(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        private bool ValidarArchivos(IEnumerable<IFormFile> archivos)
        {
            if (archivos == null)
                return true;

            var tiposPermitidos = new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", ".jpg", ".jpeg", ".png" };

            foreach (var archivo in archivos)
            {
                var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
                if (!tiposPermitidos.Contains(extension))
                {
                    return false;
                }
            }

            return true;
        }

        // Método para obtener la imagen de una receta
        public async Task<IActionResult> GetImage(int id)
        {
            var receta = await _dbContext.Recetas.FindAsync(id);
            if (receta == null || receta.Imagen == null)
            {
                return NotFound();
            }
            return File(receta.Imagen, "image/jpeg"); // Ajusta el tipo de contenido según el tipo de imagen
        }

        // Método para descargar un archivo asociado a una receta
        public async Task<IActionResult> DownloadFile(int id)
        {
            var archivo = await _dbContext.RecetaArchivos.FindAsync(id);
            if (archivo == null || archivo.Archivo == null)
            {
                return NotFound();
            }
            return File(archivo.Archivo, archivo.TipoArchivo, archivo.NombreArchivo); // Ajusta el nombre y tipo de archivo según sea necesario
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateReceta(int idReceta, int rating)
        {
            var receta = await _dbContext.Recetas.FindAsync(idReceta);
            if (receta == null)
            {
                return NotFound();
            }

            if (rating < 1 || rating > 5)
            {
                return BadRequest("La calificación debe estar entre 1 y 5.");
            }

            var userId = _userManager.GetUserId(User);

            // Buscar si el usuario ya ha calificado esta receta
            var existingRating = await _dbContext.RecetaRatings
                .FirstOrDefaultAsync(r => r.IdReceta == idReceta && r.UserId == userId);

            if (existingRating != null)
            {
                // Actualizar la calificación existente
                existingRating.Rating = rating;
                _dbContext.Update(existingRating);
            }
            else
            {
                // Agregar una nueva calificación
                var recetaRating = new RecetaRating
                {
                    IdReceta = idReceta,
                    UserId = userId,
                    Rating = rating,
                    Fecha = DateTime.Now
                };
                _dbContext.RecetaRatings.Add(recetaRating);
            }

            await _dbContext.SaveChangesAsync();

            // Calcular el nuevo promedio de calificación
            var averageRating = await _dbContext.RecetaRatings
                .Where(r => r.IdReceta == idReceta)
                .AverageAsync(r => r.Rating);

            receta.Rating = (float)averageRating;
            _dbContext.Update(receta);
            await _dbContext.SaveChangesAsync();

            return Json(new { newRating = averageRating });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MarcarComoLeida(int idReceta)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var recetaVista = await _dbContext.RecetasVistas
                .FirstOrDefaultAsync(rv => rv.IdReceta == idReceta && rv.UserId == userId);

            if (recetaVista == null)
            {
                recetaVista = new RecetaVista
                {
                    IdReceta = idReceta,
                    UserId = userId,
                    FechaVista = DateTime.Now
                };

                _dbContext.RecetasVistas.Add(recetaVista);
                await _dbContext.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MarcarComoNoLeido(int idReceta)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var recetaVista = await _dbContext.RecetasVistas
                .FirstOrDefaultAsync(rv => rv.IdReceta == idReceta && rv.UserId == userId);

            if (recetaVista != null)
            {
                _dbContext.RecetasVistas.Remove(recetaVista);
                await _dbContext.SaveChangesAsync();
            }

            return Json(new { success = true });
        }



    }
}
