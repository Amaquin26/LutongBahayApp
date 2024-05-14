using LutongBahayApp.Data;
using LutongBahayApp.Interfaces;
using LutongBahayApp.Models;
using LutongBahayApp.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LutongBahayApp.Repository
{
    public class BasketRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : IBasketRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;


        public async Task<List<BasketItemViewModel>> GetBasketItems()
        {

            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }


            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;


            if(userClaim == null)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }
            else
            {
                userId = userClaim.Value;
            }

            var basketItems = await _context.Baskets
            .Where(x => x.AppUserId == userId)
            .SelectMany(x => x.BasketFood)
            .Include(bf => bf.Food)
            .ToListAsync();

            var groupedBasketItems = basketItems
                .GroupBy(bf => bf.Food.MarketId)
                .Select(group => new BasketItemViewModel
                {
                    MarketInfo = new MarketInfoViewModel
                    {
                        MarketId = group.Key,
                        MarketName = _context.Markets.FirstOrDefault(m => m.Id == group.Key)?.Name,
                        MarketIsVerified = (bool)(_context.Markets.FirstOrDefault(m => m.Id == group.Key)?.IsVerified)
                    },
                    BasketItems = group.Select(bf => new GroupedFoodItemViewModel
                    {
                        ImagePath = bf.Food.ImagePath,
                        Name = bf.Food.Name,
                        Price = bf.Food.Price,
                        Quantity = bf.Quantity,
                        Id = bf.FoodId
                    }).ToList()
                }).ToList();

            return groupedBasketItems;
        }

        public async Task<bool> AddToBasket(int foodId, int quantity)
        {

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;

            if (userClaim == null)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }
            else
            {
                userId = userClaim.Value;
            }

            var food = _context.Foods.Where(x => x.Id == foodId).FirstOrDefault();
            var basket = _context.Baskets.Where(x => x.AppUserId == userId).FirstOrDefault();

            if(food == null || basket == null) 
            {
                return false;
            }

            var foodBasket = _context.BasketFoods.Where(x => x.FoodId == foodId && x.BasketId == basket.Id).FirstOrDefault();

            if(foodBasket == null)
            {
                BasketFood basketFood = new BasketFood
                {
                    FoodId = foodId,
                    BasketId = basket.Id,
                    LastModified = DateTime.Now,
                    Quantity = quantity,
                };

                _context.BasketFoods.Add(basketFood);
            }
            else
            {
                foodBasket.Quantity += quantity;
                foodBasket.LastModified = DateTime.Now;
            }

            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public async Task<bool> RemoveFromBasket(int id)
        {

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;

            if (userClaim == null)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }
            else
            {
                userId = userClaim.Value;
            }

            var food = _context.Foods.Where(x => x.Id == id).FirstOrDefault();
            var basket = _context.Baskets.Where(x => x.AppUserId == userId).FirstOrDefault();

            if (food == null || basket == null)
            {
                return false;
            }

            var basketFood = _context.BasketFoods.Where(x => x.FoodId == food.Id && x.BasketId == basket.Id).FirstOrDefault();

            _context.BasketFoods.Remove(basketFood);

            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public async Task<bool> UpdateQuantity(int foodId, int quantity)
        {
            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;

            if (userClaim == null)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }
            else
            {
                userId = userClaim.Value;
            }

            var food = _context.Foods.Where(x => x.Id == foodId).FirstOrDefault();
            var basket = _context.Baskets.Where(x => x.AppUserId == userId).FirstOrDefault();

            if (food == null || basket == null)
            {
                return false;
            }

            var foodBasket = _context.BasketFoods.Where(x => x.FoodId == foodId && x.BasketId == basket.Id).FirstOrDefault();

            if (foodBasket == null)
                return false;

            foodBasket.Quantity = quantity;
            foodBasket.LastModified = DateTime.Now;

            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
