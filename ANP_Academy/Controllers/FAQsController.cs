using Microsoft.AspNetCore.Mvc;

namespace ANP_Academy.Controllers
{
    public class FAQsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
