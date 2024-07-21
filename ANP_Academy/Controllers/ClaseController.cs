using ANP_Academy.DAL.Models;
using ANP_Academy.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ClaseController : Controller
{
    private readonly AnpdesarrolloContext _dbContext;

    public ClaseController(AnpdesarrolloContext dbContext)
    {
        _dbContext = dbContext;
    }

    // READ: Mostrar todas las clases
    public async Task<IActionResult> GestionClases()
    {
        return View(await _dbContext.Clases.ToListAsync());
    }

    // CREATE: Mostrar formulario de creación
    public IActionResult CreateClases()
    {
        return View();
    }

    // CREATE: Procesar formulario de creación
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateClases(ClaseViewModel model)
    {
        if (ModelState.IsValid)
        {
            var clase = new Clase
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                URLVideo = model.URLVideo,
            };

            using (var ms = new MemoryStream())
            {
                await model.Imagen.CopyToAsync(ms);
                clase.Imagen = ms.ToArray();
            }

            _dbContext.Add(clase);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(GestionClases));
        }

        return View(model);
    }

    // Método para verificar si el archivo es una imagen válida (sin GIF)
    private bool IsImageFile(IFormFile file)
    {
        string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        return !string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext);
    }

    // DETAILS: Mostrar detalles de una clase
    public async Task<IActionResult> DetailsClases(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var clase = await _dbContext.Clases
            .FirstOrDefaultAsync(m => m.IdClase == id);
        if (clase == null)
        {
            return NotFound();
        }

        return View(clase);
    }


    // UPDATE: Mostrar formulario de edición
    public async Task<IActionResult> EditClases(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var clase = await _dbContext.Clases.FindAsync(id);
        if (clase == null)
        {
            return NotFound();
        }

        var model = new ClaseViewModel
        {
            IdClase = clase.IdClase,
            Titulo = clase.Titulo,
            Descripcion = clase.Descripcion,
            URLVideo = clase.URLVideo
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditClases(int id, ClaseViewModel model)
    {
        if (id != model.IdClase)
        {
            return NotFound();
        }

        if (model.Imagen != null && model.Imagen.Length > 0)
        {
            if (!IsImageFile(model.Imagen))
            {
                ModelState.AddModelError("Imagen", "El archivo debe ser una imagen válida (jpg, jpeg, png).");
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                var clase = await _dbContext.Clases.FindAsync(id);

                if (clase == null)
                {
                    return NotFound();
                }

                clase.Titulo = model.Titulo;
                clase.Descripcion = model.Descripcion;
                clase.URLVideo = model.URLVideo;

                if (model.Imagen != null && model.Imagen.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await model.Imagen.CopyToAsync(ms);
                        clase.Imagen = ms.ToArray();
                    }
                }

                _dbContext.Update(clase);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaseExists(model.IdClase))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(GestionClases));
        }
        return View(model);
    }


    public IActionResult GetImage(int id)
    {
        var clase = _dbContext.Clases.Find(id);
        if (clase != null && clase.Imagen != null)
        {
            return File(clase.Imagen, "image/jpeg");
        }
        return NotFound();
    }


    // DELETE: Mostrar formulario de confirmación de eliminación
    public async Task<IActionResult> DeleteClases(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var clase = await _dbContext.Clases
            .FirstOrDefaultAsync(m => m.IdClase == id);
        if (clase == null)
        {
            return NotFound();
        }

        return View(clase);
    }

    // DELETE: Procesar eliminación
    [HttpPost, ActionName("DeleteClases")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteClasesConfirmed(int id)
    {
        var clase = await _dbContext.Clases.FindAsync(id);
        _dbContext.Clases.Remove(clase);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(GestionClases));
    }


    private bool ClaseExists(int id)
    {
        return _dbContext.Clases.Any(e => e.IdClase == id);
    }
}
