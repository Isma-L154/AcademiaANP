using Microsoft.AspNetCore.Mvc;

namespace ANP_Academy.Controllers
{
    public class SobreNosotrosController : Controller
    {
        public IActionResult SobreNosotros()
        {
            return View("~/Views/Shared/sobre-nosotros.cshtml");
        }
    }
}
