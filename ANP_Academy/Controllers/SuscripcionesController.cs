using Microsoft.AspNetCore.Mvc;
using ANP_Academy.DAL.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ANP_Academy.Controllers
{
    public class SuscripcionesController : Controller
    {
        private readonly AnpdesarrolloContext _dbContext;

        public SuscripcionesController(AnpdesarrolloContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Suscripciones
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Suscripciones.ToListAsync());
        }

        public IActionResult MisSuscripciones()
        {
            return View();
        }
    }
}

