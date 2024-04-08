using Microsoft.AspNetCore.Mvc;

namespace ANP_Academy.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult ControlSuscrip()
        {
            return View();
        }
        public IActionResult GestionForo()
        {
            return View();
        }

        public IActionResult GestionContenido()
        {
            return View();
        }

        public IActionResult ContenidoFormulario()
        {
            return View();
        }
    }
}
