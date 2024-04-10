using Microsoft.AspNetCore.Mvc;

namespace ANP_Academy.Controllers
{
    public class ForoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MisPublicaciones()
        {
            return View();
        }

        public IActionResult CrearPublicacion() 
        { 
            return View(); 
        }

        public IActionResult EditarPublicacion() 
        {
            return View();
        }
    }
}
