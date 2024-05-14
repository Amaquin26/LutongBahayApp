using LutongBahayApp.Data;
using LutongBahayApp.Interfaces;
using LutongBahayApp.Models;
using LutongBahayApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LutongBahayApp.Repository
{
    public class ShopRepository(ApplicationDbContext context):IShopRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<FoodListItemViewModel>> GetFoodsForShop()
        {
            var markets = await _context.Markets.ToListAsync();

            var foodsByMarket = new List<FoodListItemViewModel>();

            foreach (var market in markets)
            {
                // Retrieve foods for the current market
                var foods = await _context.Foods
                    .Where(f => f.MarketId == market.Id)
                    .ToListAsync();

                foreach (var food in foods)
                {

                    var ratings = _context.Reviews
                    .Where(r => r.FoodId == food.Id)
                    .Select(r => r.Rating)
                    .ToList();

                    double averageRating = 0;

                    if (ratings.Any())
                    {
                        averageRating = ratings.Average();
                    }

                    if (market.Name != null || market.Name != string.Empty)
                    {
                        var f = new FoodListItemViewModel
                        {
                            Id = food.Id,
                            Name = food.Name,
                            Description = food.Description,
                            ImagePath = food.ImagePath,
                            Price = food.Price,
                            MarketName = market.Name,
                            MarketId = food.MarketId,
                            Rating = averageRating
                        };

                        foodsByMarket.Add(f);
                    }

                }
            }

            return foodsByMarket;
        }

        public async Task<FoodItemViewModel> GetIndividualFood(int id)
        {
            var food = await _context.Foods.Include(f => f.Market).Where(f => f.Id == id).FirstOrDefaultAsync();

            if (food == null)
                return null;

            var foodReviews = _context.Reviews
                .Include(r => r.User)
                .OrderBy(r => r.HelpfulCount)
                .ThenBy(r => r.DateCreated)
                .Where(r => r.FoodId == id)
                .ToList();

            double averageRating = 0;

            if (foodReviews.Any())
            {
                averageRating = foodReviews.Select(r => r.Rating).Average();
            }

            var market = _context.Markets.Include(m => m.Foods).Where(m => m.Id == food.Market.Id).FirstOrDefault();

            if (market == null)
                market = food.Market;

            var foodIds = market.Foods.Select(f => f.Id).ToList();

            var marketReviewsCount = _context.Reviews
                .Include(r => r.User)
                .Where(r => foodIds.Contains(r.FoodId))
                .OrderBy(r => r.HelpfulCount)
                .ThenBy(r => r.DateCreated)
                .Count();

            var foodItem = new FoodItemViewModel
            {
                Id = food.Id,
                Name = food.Name,
                Description = food.Description,
                ImagePath = food.ImagePath,
                Market = market,
                Price = food.Price,
                Rating = averageRating,
                Reviews = foodReviews,
                MarketTotalReviews = marketReviewsCount
            };

            return foodItem;
        }

        public async Task<MarketPageInfoViewModel> GetMarketPageInfo(int id, string foodName = null, int foodId = 0)
        {
            var market = await _context.Markets.Include(m => m.Foods).Where(m => m.Id == id).FirstOrDefaultAsync();

            if (market == null)
                return null;

            var foodIds = market.Foods.Select(f => f.Id).ToList();

            var reviewsCount = _context.Reviews
            .Include(r => r.User)
            .Where(r => foodIds.Contains(r.FoodId))
            .OrderBy(r => r.HelpfulCount)
            .ThenBy(r => r.DateCreated)
            .Count();

            var foods = new List<FoodListItemViewModel>();
            foreach( var food in market.Foods)
            {
                var ratings = _context.Reviews
                    .Where(r => r.FoodId == food.Id)
                    .Select(r => r.Rating)
                    .ToList();

                double averageRating = 0;

                if (ratings.Any())
                {
                    averageRating = ratings.Average();
                }

                var f = new FoodListItemViewModel
                {
                    Id = food.Id,
                    Name = food.Name,
                    Description = food.Description,
                    ImagePath = food.ImagePath,
                    Price = food.Price,
                    MarketName = market.Name,
                    MarketId = food.MarketId,
                    Rating = averageRating
                };

                foods.Add(f);
            }

            var marketInfo = new MarketPageInfoViewModel
            {
                Id = market.Id,
                Name = market.Name,
                Address = market.Address,
                Location = market.Location,
                DateCreated = market.DateCreated,
                Foods = foods,
                IsVerified = market.IsVerified,
                TotalProducts = market.Foods.Count(),
                TotalReviews = reviewsCount
            };

            if(foodName != null && foodId != 0)
            {
                marketInfo.FoodId = foodId;
                marketInfo.FoodName = foodName;
            }

            return marketInfo;
        }
    }
}
