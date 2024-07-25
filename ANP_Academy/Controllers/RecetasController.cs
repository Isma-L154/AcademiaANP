using ANP_Academy.DAL.Models;
using ANP_Academy.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ANP_Academy.Controllers
{
    public class RecetasController : Controller
    {
        private readonly AnpdesarrolloContext _dbContext;

        public RecetasController(AnpdesarrolloContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var recetas = await _dbContext.Recetas.ToListAsync();
            return View(recetas);
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




        public async Task<IActionResult> GestionRecetas()
        {
            var recetas = await _dbContext.Recetas
                .Include(r => r.Archivos)
                .ToListAsync();
            return View(recetas);
        }

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
                if (!ValidarArchivos(model.ArchivosNuevos))
                {
                    ModelState.AddModelError("ArchivosNuevos", "Los archivos subidos no son válidos.");
                    return View(model);
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
                return RedirectToAction(nameof(GestionRecetas));
            }

            return View(model);
        }

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
                if (!ValidarArchivos(model.ArchivosNuevos))
                {
                    ModelState.AddModelError("ArchivosNuevos", "Los archivos subidos no son válidos.");
                    return View(model);
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
    }
}
