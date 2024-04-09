using Microsoft.AspNetCore.Mvc;

namespace ANP_Academy.Controllers
{
    public class UsuarioController : Controller
    {  
            // Action for the Signup view
            public IActionResult Signup()
            {
                return View();
            }

            public IActionResult SolicitudEnviada()
            {
                return View();
            }
    }
}
