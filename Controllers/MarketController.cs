using LutongBahayApp.Interfaces;
using LutongBahayApp.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Models;
using System.Security.Claims;

namespace LutongBahayApp.Controllers
{
    public class MarketController(IMarketRepository marketRepository, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor) : Controller
    {
        private readonly IMarketRepository _marketRepository = marketRepository;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public async Task<IActionResult> Dashboard()
        {
            bool response = await IsUserAuthenticatedAndSeller();

            if (!response)
                return RedirectToAction("Unauthorized", "Errors");

            var marketDashboard = await _marketRepository.GetUserMarket();
            return View(marketDashboard);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name, string location, string landmark)
        {
            bool isSeller = await IsUserAuthenticatedAndSeller();

            if (!isSeller)
                return RedirectToAction("Unauthorized", "Errors");

            var response = await _marketRepository.AddMarket(name, location, landmark);

            if (response == -1)
                return RedirectToAction("AddMarketError");

            return RedirectToAction("Manage", new { id = response });
        }

        public async Task<IActionResult> Manage(int id)
        {
            bool isSeller = await IsUserAuthenticatedAndSeller();

            if (!isSeller)
                return RedirectToAction("Unauthorized", "Errors");

            bool response = await IsUserAuthenticatedAndSeller();

            if (!response)
                return RedirectToAction("Unauthorized", "Errors");

            var marketInfo = await _marketRepository.GetMarketPageInfo(id);

            if(!marketInfo.Item1)
                return RedirectToAction("Unauthorized", "Errors");

            return View(marketInfo.Item2);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string location, string landmark)
        {
            bool isSeller = await IsUserAuthenticatedAndSeller();

            if (!isSeller)
                return RedirectToAction("Unauthorized", "Errors");

            var response = await _marketRepository.EditMarket(id, name, location, landmark);

            if (response.Item1 == false)
                return RedirectToAction("Unauthorized", "Errors");

            if(response.Item2 == -1)
                return RedirectToAction("EditMarketError");

            return RedirectToAction("Manage", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddFood(int id, string name, decimal price, string description, IFormFile filepath)
        {
            bool isSeller = await IsUserAuthenticatedAndSeller();

            if (!isSeller)
                return RedirectToAction("Unauthorized", "Errors");

            var response = await _marketRepository.AddFoodToMarket(id, name, price, description, filepath);

            if (response.Item1 == false)
                return RedirectToAction("Unauthorized", "Errors");

            if (response.Item2 == -1)
                return RedirectToAction("AddFoodError");

            return RedirectToAction("Manage", new { id = id });
        }

        public async Task<IActionResult> EditFood(int marketId, int foodId)
        {
            var isSeller = await IsUserAuthenticatedAndSeller();

            if(!isSeller)
                return RedirectToAction("Unauthorized", "Errors");

            var response = await _marketRepository.VerifyFoodOwnerShip(marketId, foodId);

            if(response == -1)
                return RedirectToAction("Unauthorized", "Errors");

            var model = await _marketRepository.GetFoodManageInfo(foodId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditFoodDetail(int marketId,int foodId, string name, decimal price, string description, IFormFile filepath)
        {
            bool isSeller = await IsUserAuthenticatedAndSeller();

            if (!isSeller)
                return RedirectToAction("Unauthorized", "Errors");

            var response = await _marketRepository.EditFoodInMarket(marketId, foodId, name, price, description, filepath);

            if (response.Item1 == false)
                return RedirectToAction("Unauthorized", "Errors");

            if (response.Item2 == -1)
                return RedirectToAction("AddFoodError");

            return RedirectToAction("EditFood", new { marketId = marketId, foodId = foodId });
        }

        public async Task<IActionResult> ChangeStatus(int foodId, int orderId, int flag)
        {
            await _marketRepository.ChangeStatus(foodId, orderId, flag);

            return RedirectToAction("MarketOrders");
        }

        public IActionResult AddMarketError()
        {
            return View();
        }
        public IActionResult AddFoodError()
        {
            return View();
        }
        public IActionResult EditMarketError()
        {
            return View();
        }
        
        public async Task<IActionResult> MarketOrders()
        {
            bool response = await IsUserAuthenticatedAndSeller();

            if (!response)
                return RedirectToAction("Unauthorized", "Errors");

            var orderFoods = await _marketRepository.GetMarketFoodOrders();

            return View(orderFoods);
        }

        private async Task<bool> IsUserAuthenticatedAndSeller()
        {

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
                return false;

            string userId = userClaim.Value;         

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("Seller"))
                return false;

            return true;
        }
    }
}
    