using LutongBahayApp.Interfaces;
using LutongBahayApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LutongBahayApp.Controllers
{
    public class ShopController(IShopRepository shopRepository) : Controller
    {
        private readonly IShopRepository _shopRepository = shopRepository;
        public async Task<IActionResult> Index()
        {
            var foodList = await _shopRepository.GetFoodsForShop();
            return View(foodList);
        }

        public async Task<IActionResult> Food(int id)
        {
            var food = await _shopRepository.GetIndividualFood(id);

            if(food == null)
            {
                return View("FoodNotFound");
            }

            return View(food);
        }

        public async Task<IActionResult> Market(int id,string foodName = null,int foodId = 0)
        {
            MarketPageInfoViewModel market;
            if ((foodName == null || foodName == string.Empty) || foodId == 0)
                market = await _shopRepository.GetMarketPageInfo(id);
            else
                market = await _shopRepository.GetMarketPageInfo(id, foodName, foodId);

            if (market == null)
            {
                return View("MarketNotFound");
            }

            return View(market);
        }
    }
}
