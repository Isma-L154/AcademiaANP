using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ANP_Academy.DAL.Models;

namespace ANP_Academy.Controllers
{
    public class FacturasController : Controller
    {
        private readonly AnpdesarrolloContext _dbContext;

        public FacturasController(AnpdesarrolloContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerFacturasUsuario()
        {
            return View();
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
