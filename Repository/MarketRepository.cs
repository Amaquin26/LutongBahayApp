using LutongBahayApp.Data;
using LutongBahayApp.Data.Enum;
using LutongBahayApp.Interfaces;
using LutongBahayApp.Models;
using LutongBahayApp.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCWebApp.Models;
using System.Linq;
using System.Security.Claims;

namespace LutongBahayApp.Repository
{
    public class MarketRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IWebHostEnvironment  environment) :IMarketRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;  
        private readonly IWebHostEnvironment _environment = environment;  

        public async Task<List<MarketDashboardViewModel>> GetUserMarket()
        {
            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userClaim.Value;
            var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

            var markets = _context.Markets.Include(x => x.Foods).Where(x => x.UserId == userId).ToList();
            List<MarketDashboardViewModel> result = new List<MarketDashboardViewModel>();

            foreach (var market in markets)
            {
                var reviewsCount = _context.Reviews
                    .Where(r => market.Foods.Select(x => x.Id).Contains(r.FoodId))
                    .Count();

                var m = new MarketDashboardViewModel
                {
                    MarketName = market.Name,
                    MarketId = market.Id,
                    DateCreated = market.DateCreated,
                    MarketIsVerified = market.IsVerified,
                    TotalProducts = market.Foods.Count,
                    TotalReviews = reviewsCount,
                    MarketAddress = market.Address
                };

                result.Add(m);
                // PLEASE ADD SOLDPRODUCTS
            }

            return result;
        }

        public async Task<int> AddMarket(string name,string address,string landmark)
        {
            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;

            if (userClaim == null)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }

            userId = userClaim.Value;

            var market = new Market
            {
                Name = name,
                Address = address,
                Location = landmark,
                DateCreated = DateTime.Now,
                UserId = userId,
                IsVerified = false
            };

            _context.Markets.Add(market);

            var saved = _context.SaveChanges();
            return saved > 0 ? market.Id : -1;
        }

        public async Task<(bool, MarketPageInfoViewModel)> GetMarketPageInfo(int id)
        {
            // Validate if the client user owns the market with this id

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
                return (false, null);

            string userId = userClaim.Value;

            var market = await _context.Markets.Include(m => m.Foods).Where(m => m.Id == id && m.UserId == userId).FirstOrDefaultAsync();

            if (market == null)
                return new (false, null);

            var foodIds = market.Foods.Select(f => f.Id).ToList();

            var reviewsCount = _context.Reviews
            .Include(r => r.User)
            .Where(r => foodIds.Contains(r.FoodId))
            .OrderBy(r => r.HelpfulCount)
            .ThenBy(r => r.DateCreated)
            .Count();

            var foods = new List<FoodListItemViewModel>();
            foreach (var food in market.Foods)
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

            return (true, marketInfo);
        }

        public async Task<(bool, int)> EditMarket(int id, string name, string address, string landmark)
        {
            // Validate if the client user owns the market with this id

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
                return (false, -1);

            string userId = userClaim.Value;

            var market = await _context.Markets.Include(m => m.Foods).Where(m => m.Id == id && m.UserId == userId).FirstOrDefaultAsync();

            if (market == null)
                return new(false, -1);

            market.Name = name;
            market.Address = address;
            market.Location = landmark;

            var saved = _context.SaveChanges();
            return (true, 1);
        }

        public async Task<(bool, int)> AddFoodToMarket(int id, string name, decimal price, string description, IFormFile filepath)
        {
            // Validate if the client user owns the market with this id

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
                return (false, -1);

            string userId = userClaim.Value;

            var market = await _context.Markets.Include(m => m.Foods).Where(m => m.Id == id && m.UserId == userId).FirstOrDefaultAsync();

            if (market == null)
                return new(false, -1);

            string foodImagePath = "/images//no_food_image.png";
            if (filepath != null && filepath.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "foods", userId, market.Id.ToString());
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = $"{Path.GetRandomFileName()}{Path.GetExtension(filepath.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await filepath.CopyToAsync(fileStream);
                }

                foodImagePath = $"/images//foods//{userId}//{market.Id}//{uniqueFileName}";
            }

            Food food = new Food
            {
                Name = name,
                Price = price,
                Description = description,
                ImagePath = foodImagePath,
                MarketId = id
            };

            _context.Foods.Add(food);

            var saved = _context.SaveChanges();
            return (true, 1);
        }

        public async Task<ManageFoodViewModel> GetFoodManageInfo(int foodId)
        {
            var manageFoodViewModel = new ManageFoodViewModel();

            var food = await _context.Foods.Include(f => f.Market).Where(f => f.Id == foodId).FirstOrDefaultAsync();

            var foodReviews = _context.Reviews
                .Include(r => r.User)
                .OrderBy(r => r.HelpfulCount)
                .ThenBy(r => r.DateCreated)
                .Where(r => r.FoodId == foodId)
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

            var endDate = DateTime.Today;
            var startDate = endDate.AddDays(-4);

            //Get Sold Food Metric
            var foodMetrics = _context.SoldFoodMetrics
            .Where(sp => (sp.DateSold.Date >= startDate && sp.DateSold.Date <= endDate) && sp.FoodId == foodId)
            .ToList();

            var totalSold = _context.SoldFoodMetrics.Where(x => x.FoodId == foodId).Select(x => x.SoldCount).Sum(); 

            //Get Metric Growth
            // Calculate daily growth/decline percentages
            var soldCounts = foodMetrics.Select(x => x.SoldCount).ToList();
            int startSales = soldCounts.Any() ? soldCounts[0] : 0;
            int endSales = soldCounts.Any() ?  soldCounts[soldCounts.Count() - 1] : 0;

            double soldGrowthRate = startSales == 0 ? 0 : ((endSales - startSales) / (double)startSales) * 100;

            //Get Price Change Metric
            var priceChange = _context.FoodPriceChanges
            .Where(sp => (sp.DateChanged.Date >= startDate && sp.DateChanged.Date <= endDate) && sp.FoodId == foodId)
            .ToList();

            //Get Metric Growth
            // Calculate daily growth/decline percentages
            var priceChanges = priceChange.Select(x => x.Price).ToList();
            decimal startPrice = priceChanges.Any() ? priceChanges[0] : 0;
            decimal endPrice = priceChanges.Any() ? priceChanges[priceChanges.Count() - 1] : 0;
            double priceGrowthRate = startPrice == 0 ? 0 : (double)((endPrice - startPrice) / startPrice) * 100;

            manageFoodViewModel.FoodItemViewModel = foodItem;
            manageFoodViewModel.FoodPriceChange = priceChange;
            manageFoodViewModel.SoldFoodMetric = foodMetrics;
            manageFoodViewModel.SalesGrowth = soldGrowthRate;
            manageFoodViewModel.PriceGrowth = priceGrowthRate;
            manageFoodViewModel.TotalSold = totalSold;
            manageFoodViewModel.TotalSales = food.Sales;
            return manageFoodViewModel;
        }

        public async Task<List<OrderFoodItemsViewModel>> GetMarketFoodOrders()
        {
            List<OrderFoodItemsViewModel> orderFoodItems = new List<OrderFoodItemsViewModel>();

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userClaim.Value;
            var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

            var markets = await _context.Markets.Where(x => x.UserId == userId).Select(x => x.Id).ToListAsync();
            var foodIds = await _context.Foods.Where(x => markets.Contains(x.MarketId)).Select(x => x.Id).ToListAsync();
            var orderIds = await _context.Orders.Include(x => x.OrderFood).Where(x => x.Status == OrderStatus.Pending).Select(x => x.Id).ToListAsync();
            var orderFoods = _context.OrderFoods.Include(x => x.Food).Where(x => orderIds.Contains(x.OrderId) && foodIds.Contains(x.FoodId) && x.Status == FoodOrderStatus.Pending).ToList();
            
            foreach (var food in orderFoods)
            {
                var orderFoodItem = new OrderFoodItemsViewModel
                {
                    Id = food.Food.Id,
                    OrderId = food.OrderId,
                    Name = food.Food.Name,
                    ImagePath = food.Food.ImagePath,
                    Quantity = food.Quantity,
                    Price = food.Food.Price
                };

                orderFoodItems.Add(orderFoodItem);
            }

            return orderFoodItems;
        }

        public async Task<int> ChangeStatus(int foodId, int orderId, int flag)
        {
            //var orderFood = await _context.OrderFoods.Where(x => x.FoodId == foodId && x.OrderId == orderId).FirstOrDefaultAsync();
            var order = await _context.Orders.Include(x => x.OrderFood).Where(x => x.Id == orderId).FirstOrDefaultAsync();

            if (order == null)
            {
                return -1;
            }

            var orderedFood = order.OrderFood.Where(x => x.FoodId == foodId).FirstOrDefault();

            if (orderedFood == null) return -1;

            if (flag == 0)
            {
                
                orderedFood.Status = FoodOrderStatus.Declined;
                order.Status = OrderStatus.Cancelled;
                await _context.SaveChangesAsync();
            }
            else
            {
                orderedFood.Status = FoodOrderStatus.Accepted;
                await _context.SaveChangesAsync();

                var orderCounts = order.OrderFood.Count();
                var orderedAccepted = order.OrderFood.Where(x => x.Status == FoodOrderStatus.Accepted).Count();

                if(orderCounts == orderedAccepted)
                {
                    order.Status = OrderStatus.OnDelivery;
                    await _context.SaveChangesAsync();
                }

            }

            return 1;
        }

        public async Task<int> VerifyFoodOwnerShip(int marketId, int foodId)
        {
            // Validate if the client user owns the market with this id

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
                return -1; //return not authenticated

            string userId = userClaim.Value;

            var market = await _context.Markets.Include(m => m.Foods).Where(m => m.Id == marketId && m.UserId == userId).FirstOrDefaultAsync();

            if (market == null)
                return -1; // not his market

            var food = market.Foods.Where(x => x.Id == foodId).FirstOrDefault();

            if (food == null)
                return -1; // he does not own this food

            return 1;
        }

        public async Task<(bool, int)> EditFoodInMarket(int marketId,int foodId, string name, decimal price, string description, IFormFile filepath)
        {
            // Validate if the client user owns the market with this id

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
                return (false, -1);

            string userId = userClaim.Value;

            var market = await _context.Markets.Include(m => m.Foods).Where(m => m.Id == marketId && m.UserId == userId).FirstOrDefaultAsync();

            if (market == null)
                return new(false, -1);

            var food = market.Foods.Where(x => x.Id == foodId).FirstOrDefault();

            if(food == null)
               return new(false, -1);

            string foodImagePath = food.ImagePath;
            if (filepath != null && filepath.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "foods", userId, market.Id.ToString());
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = $"{Path.GetRandomFileName()}{Path.GetExtension(filepath.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await filepath.CopyToAsync(fileStream);
                }

                foodImagePath = $"/images//foods//{userId}//{market.Id}//{uniqueFileName}";
            }

            food.Name = name ?? food.Name;
            food.Price = price;
            food.Description = description ?? food.Description;
            food.ImagePath = foodImagePath;

            var priceChange = await _context.FoodPriceChanges.Where(x => x.FoodId == foodId && x.DateChanged.Date == DateTime.Today.Date).FirstOrDefaultAsync();

            if (priceChange != null)
            {
                priceChange.Price = price;
            }
            else
            {
                var p = new FoodPriceChange
                {
                    FoodId = foodId,
                    Price = price,
                    DateChanged = DateTime.Today.Date,
                };

                _context.FoodPriceChanges.Add(p);
            }

            var saved = _context.SaveChanges();
            return (true, 1);
        }
    }
}
