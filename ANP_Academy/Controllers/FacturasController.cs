using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ANP_Academy.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace ANP_Academy.Controllers
{
    public class FacturasController : Controller
    {
        private readonly AnpdesarrolloContext _dbContext;
        private readonly UserManager<Usuario> _userManager;

        public FacturasController(AnpdesarrolloContext dbContext, UserManager<Usuario> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> VerFacturasUsuario()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var facturas = await _dbContext.Facturas
                .Where(f => f.IdUser == user.Id)
                .Include(f => f.Suscripcion)
                .ToListAsync();

            if (facturas == null || facturas.Count == 0)
            {
                ViewBag.Message = "No tiene facturas asociadas a su cuenta.";
            }

            return View(facturas);
        }

        public async Task<IActionResult> VerDetalleFacturaUsuario(int id)
        {
            var factura = await _dbContext.Facturas
                .Include(f => f.Usuario)
                .Include(f => f.Suscripcion)
                .FirstOrDefaultAsync(f => f.IdFactura == id);

            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        public async Task<IActionResult> VerDetalleFacturaAdmin(int id)
        {
            var factura = await _dbContext.Facturas
                .Include(f => f.Usuario)
                .Include(f => f.Suscripcion)
                .FirstOrDefaultAsync(f => f.IdFactura == id);

            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }
    }
}
