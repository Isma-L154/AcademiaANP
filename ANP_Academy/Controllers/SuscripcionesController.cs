using Microsoft.AspNetCore.Mvc;

namespace ANP_Academy.Controllers
{
    public class SuscripcionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MisSuscripciones()
        {
            return View();
        }
    }
}
