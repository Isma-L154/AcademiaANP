using Microsoft.AspNetCore.Mvc;

namespace ANP_Academy.Controllers
{
    public class FacturasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerFacturasUsuario()
        {
            return View();
        }

        public IActionResult VerDetalleFacturaUsuario()
        {
            return View();
        }

        public IActionResult VerDetalleFacturaAdmin()
        {
            return View();
        }
    }
}
