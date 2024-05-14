using LutongBahayApp.Interfaces;
using LutongBahayApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LutongBahayApp.Controllers
{
    public class AdminController(IAdminRepository adminRepository,UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor) : Controller
    {

        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IAdminRepository _adminRepository = adminRepository;

        public IActionResult Panel()
        {
            return View();
        }

        public IActionResult Markets()
        {
            return View();
        }

        public IActionResult Issues()
        {
            return View();
        }

        public async Task<IActionResult> OnDelivery()
        {
            bool response = await IsUserAuthenticatedAndAdmin();

            if (!response)
                return RedirectToAction("Unauthorized", "Errors");

            var orders = await _adminRepository.GetOnDeliveryOrders();

            return View(orders);
        }

        public async Task<IActionResult> SetMarketDelivery(int flag, int orderId)
        {
            bool response = await IsUserAuthenticatedAndAdmin();

            if (!response)
                return RedirectToAction("Unauthorized", "Errors");

            await _adminRepository.SetMarketDelivery(flag, orderId);

            return RedirectToAction("OnDelivery");
        }

        private async Task<bool> IsUserAuthenticatedAndAdmin()
        {

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
                return false;

            string userId = userClaim.Value;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("Admin"))
                return false;

            return true;
        }
    }
}
