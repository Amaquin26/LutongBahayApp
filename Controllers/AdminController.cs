using Microsoft.AspNetCore.Mvc;

namespace LutongBahayApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Panel()
        {
            return View();
        }
    }
}
