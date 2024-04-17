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

         public IActionResult CuentaCreada()
         {
            return View();
         }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult EditarPerfil()
        {
            return View();
        }
    }
}
