using LutongBahayApp.Interfaces;
using LutongBahayApp.Models;
using LutongBahayApp.Repository;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace LutongBahayApp.Controllers
{
    public class HomeController(UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor) : Controller
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public async Task<IActionResult> Index()
        {

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
            {
                return View(); 
            }

            var user = await _userManager.FindByIdAsync(userClaim.Value);

            if(user == null)
            {
                return View();
            }

            var roles = await _userManager.GetRolesAsync(user);

            if(roles == null)
            {
                return View();
            }

            if (roles.Contains("Admin"))
            {
                return RedirectToAction("Panel","Admin");
            }else if (roles.Contains("Seller"))
            {
                return RedirectToAction("Dashboard", "Market");
            }
            else if (roles.Contains("Customer"))
            {
                return View();
            }
            else
            {
                return View();
            }          
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
