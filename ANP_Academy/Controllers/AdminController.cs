using Microsoft.AspNetCore.Mvc;

namespace ANP_Academy.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult DashBoard()
        {
            return View();
        }

   
    }
}
