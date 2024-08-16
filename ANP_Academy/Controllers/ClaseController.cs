using ANP_Academy.DAL.Models;
using ANP_Academy.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;

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
}


