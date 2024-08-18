using ANP_Academy.DAL.Models;
using ANP_Academy.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using ANP_Academy.ViewModel.Foro;
using ANP_Academy.ViewModel.Clases;

public class ClaseController : Controller
{
    private readonly AnpdesarrolloContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly UserManager<Usuario> _userManager;

    public ClaseController(UserManager<Usuario> userManager, AnpdesarrolloContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _dbContext.Clases.ToListAsync());
    }

    public async Task<IActionResult> GestionClases()
    {
        return View(await _dbContext.Clases.ToListAsync());
    }

    public IActionResult CreateClases()
    {
        return View();
    }

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

            // Enviar notificación por correo electrónico a los estudiantes suscritos
            await NotificarEstudiantesSuscritos(clase);

            return RedirectToAction(nameof(GestionClases));
        }

        return View(model);
    }

    private async Task NotificarEstudiantesSuscritos(Clase clase)
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
            string asunto = "Nueva Clase Agregada";
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
                <p>Se ha agregado una nueva clase titulada <strong>{clase.Titulo}</strong> en <strong>ANP Academy</strong>.</p>
                <p>Descripción: {clase.Descripcion}</p>
                <a href='{clase.URLVideo}' class='btn'>Ver Clase</a>
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

    public async Task<IActionResult> DetailsClases(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var clase = await _dbContext.Clases.FirstOrDefaultAsync(m => m.IdClase == id);
        if (clase == null)
        {
            return NotFound();
        }

        return View(clase);
    }

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

    public async Task<IActionResult> DeleteClases(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var clase = await _dbContext.Clases.FirstOrDefaultAsync(m => m.IdClase == id);
        if (clase == null)
        {
            return NotFound();
        }

        return View(clase);
    }

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

    private bool IsImageFile(IFormFile file)
    {
        string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        return !string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext);
    }


    //Metodos para que los usuarios puedan realizar un CRUD con comentarios a las Clases

    [Authorize]
    //Vizualizar comentarios de una publicacion
    public async Task<IActionResult> ComentariosClase(int Id)
    {
        var identidadUsuario = User.Identity as ClaimsIdentity;
        string userId = identidadUsuario.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        ViewData["UserId"] = userId;
        //Esto se hace para la paginacion
        var Clase = await _dbContext.Clases.FirstOrDefaultAsync(m => m.IdClase == Id);
        var comentarios = await _dbContext.ClaseComentario.Include(c => c.CodigoUsuario).ToListAsync();
        var ComentarioClase = await _dbContext.ClaseXComentario.ToListAsync();

        var viewModel = new ComentarioClasesViewModel
        {
            Clase = Clase,
            Comentarios = comentarios,
            ComentariosClases = ComentarioClase,
        };

        return View(viewModel);
    }

    //POST CrearComentario
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> CrearComentario([Bind("ContenidoComentario")] ClaseComentario comentario, int IdClase)
    {
        var identidad = User.Identity as ClaimsIdentity;
        string idUsuarioLoggeado = identidad.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

        if (idUsuarioLoggeado == null)
        {
            return Unauthorized();
        }

        comentario.CodigoUsuarioId = idUsuarioLoggeado;
        comentario.FechaComentario = DateTime.Now;

        if (ModelState.IsValid)
        {
            _dbContext.Add(comentario);
            await _dbContext.SaveChangesAsync();
            TempData["NuevoComentarioId"] = comentario.IdComentario;

            var ClaseXComentarios = new ClaseXComentarios
            {
                ClaseId = IdClase,
                ComentarioId = comentario.IdComentario
            };

            _dbContext.Add(ClaseXComentarios);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(ComentariosClase), new { Id = IdClase });

        }
        return RedirectToAction(nameof(ComentariosClase), new { Id = IdClase });

    }

    //Editar Un Comentario de una clase
    [HttpPost, ActionName("EditComentario")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(int idClase, int idComentario, [Bind("ContenidoComentario")] ClaseComentario claseComentario)
    {
        var existingComentario = await _dbContext.ClaseComentario.FindAsync(idComentario);

        if (existingComentario == null)
        {
            return NotFound();
        }

        existingComentario.ContenidoComentario = claseComentario.ContenidoComentario;
        existingComentario.FechaComentario = DateTime.Now;

        if (ModelState.IsValid)
        {
            try
            {
                
                _dbContext.Update(existingComentario);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                if (!ClaseComentarioExists(idComentario))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return RedirectToAction(nameof(ComentariosClase), new { Id = idClase });
        }
        
        return RedirectToAction(nameof(ComentariosClase), new { Id = idClase });
    }


    //Delete para eliminar un comentario tanto de la tabla de enlace como la de tabla original
    [HttpPost, ActionName("DeleteComentario")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id, int idClase)
    {
        var claseComentario = await _dbContext.ClaseComentario.FindAsync(id);
        var comentarioEnlace = await _dbContext.ClaseXComentario
                .FirstOrDefaultAsync(c => c.ComentarioId == id && c.ClaseId == idClase);
        
        if (claseComentario != null && comentarioEnlace != null)
        {
            _dbContext.ClaseXComentario.Remove(comentarioEnlace);   
            _dbContext.ClaseComentario.Remove(claseComentario);
            await _dbContext.SaveChangesAsync();
        }

        return RedirectToAction(nameof(ComentariosClase), new { Id = idClase });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> RateClass(int idClase, int rating)
    {
        var clase = await _dbContext.Clases.FindAsync(idClase);
        if (clase == null)
        {
            return NotFound();
        }

        if (rating < 1 || rating > 5)
        {
            return BadRequest("La calificación debe estar entre 1 y 5.");
        }

        var userId = _userManager.GetUserId(User);

        // Buscar si el usuario ya ha calificado esta clase
        var existingRating = await _dbContext.ClaseRatings
            .FirstOrDefaultAsync(r => r.IdClase == idClase && r.UserId == userId);

        if (existingRating != null)
        {
            // Actualizar la calificación existente
            existingRating.Rating = rating;
            _dbContext.Update(existingRating);
        }
        else
        {
            // Agregar una nueva calificación
            var claseRating = new ClaseRating
            {
                IdClase = idClase,
                UserId = userId,
                Rating = rating,
                Fecha = DateTime.Now
            };
            _dbContext.ClaseRatings.Add(claseRating);
        }

        await _dbContext.SaveChangesAsync();

        // Calcular el nuevo promedio de calificación
        var averageRating = await _dbContext.ClaseRatings
            .Where(r => r.IdClase == idClase)
            .AverageAsync(r => r.Rating);

        clase.Rating = (float)averageRating;
        _dbContext.Update(clase);
        await _dbContext.SaveChangesAsync();

        return Json(new { newRating = averageRating });
    }



    //Validar que exista el comentario
    private bool ClaseComentarioExists(int id)
    {
        return _dbContext.ClaseComentario.Any(e => e.IdComentario == id);
    }

}


