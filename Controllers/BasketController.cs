using LutongBahayApp.Interfaces;
using LutongBahayApp.Models;
using LutongBahayApp.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;

namespace LutongBahayApp.Controllers
{
    public class BasketController(IBasketRepository basketRepository) : Controller
    {
        private readonly IBasketRepository _basketRepository = basketRepository;

        public async Task<IActionResult> Index()
        {
            var basketItems = await _basketRepository.GetBasketItems();
            return View(basketItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket([FromBody] AddToBasketViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Failed to add item to basket");

            bool response = await _basketRepository.AddToBasket(model.FoodId, model.Quantity);

            if (!response)
                return StatusCode(500);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> QuantityChange([FromBody] AddToBasketViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Failed to update Quantity");

            bool response = await _basketRepository.UpdateQuantity(model.FoodId, model.Quantity);

            if (!response)
                return StatusCode(500);

            return Ok();
        }

        public async Task<IActionResult> RemoveFood(int foodId)
        {
            bool response = await _basketRepository.RemoveFromBasket(foodId);

            if (!response)
            {
                //return StatusCode(500);
                return View("ErrorAddToBasket");
            }

            return RedirectToAction("Index", "Basket");
            //return Ok();
        }
    }
}
