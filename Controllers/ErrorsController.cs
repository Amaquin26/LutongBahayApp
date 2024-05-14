using Microsoft.AspNetCore.Mvc;

namespace LutongBahayApp.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Unauthorized()
        {
            return View();
        }

        public IActionResult NotFound()
        {
            return View();
        }
    }
}
