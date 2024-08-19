using ANP_Academy.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class NotificacionesController : Controller
{
    private readonly UserManager<Usuario> _userManager;
    private readonly AnpdesarrolloContext _dbContext;

    public NotificacionesController(UserManager<Usuario> userManager, AnpdesarrolloContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> MisNotificaciones()
    {
        string userId = _userManager.GetUserId(User);
        var notificaciones = await _dbContext.Notificacion
            .Where(n => n.IdUser == userId)
            .ToListAsync();

        return View(notificaciones);
    }

    [HttpPost]
    public async Task<IActionResult> MarcarComoLeido(int id)
    {
        var notificacion = await _dbContext.Notificacion.FindAsync(id);
        if (notificacion != null)
        {
            notificacion.EsLeido = true;
            await _dbContext.SaveChangesAsync();
        }
        return RedirectToAction("MisNotificaciones");
    }

    [HttpPost]
    public async Task<IActionResult> MarcarComoNoLeido(int id)
    {
        var notificacion = await _dbContext.Notificacion.FindAsync(id);
        if (notificacion != null)
        {
            notificacion.EsLeido = false;
            await _dbContext.SaveChangesAsync();
        }
        return RedirectToAction("MisNotificaciones");
    }

    [HttpPost]
    public async Task<IActionResult> EliminarNotificacion(int id)
    {
        var notificacion = await _dbContext.Notificacion.FindAsync(id);
        if (notificacion != null)
        {
            _dbContext.Notificacion.Remove(notificacion);
            await _dbContext.SaveChangesAsync();
        }
        return RedirectToAction("MisNotificaciones");
    }
}

