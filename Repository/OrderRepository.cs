using LutongBahayApp.Data;
using LutongBahayApp.Data.Enum;
using LutongBahayApp.Interfaces;
using LutongBahayApp.Models;
using LutongBahayApp.ViewModels;
using LutongBahayApp.ViewModels.Post;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;

namespace LutongBahayApp.Repository
{
    public class OrderRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : IOrderRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public async Task<bool> CheckOutFoods(CheckOutViewModel checkout)
        {
            if (checkout == null || checkout.FoodBaskets == null || !checkout.FoodBaskets.Any())
            {
                return false;
            }

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;

            if (userClaim == null)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }

            userId = userClaim.Value;

            var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            string orderAddress = String.IsNullOrEmpty(checkout.OrderAddress) ? user.Address : checkout.OrderAddress;

            // Create order first
            var newOrder = new Order
            {
                DateCreated = DateTime.Now,
                Status = OrderStatus.Pending,
                OrderAdress = orderAddress,
                OrderAmountDue = checkout.TotalDue,
                AppUserId = userId,
            };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // Find the basket
            var basket = _context.Baskets.Where(x => x.AppUserId == userId).FirstOrDefault();

            if (basket == null)
                return false;

            // Add each food to the order
            foreach(var food in checkout.FoodBaskets)
            {
                var orderedFood = new OrderFood
                {
                    OrderId = newOrder.Id,
                    FoodId = food.FoodId,
                    Quantity = food.Quantity,
                    Price = food.Price,
                };

                _context.OrderFoods.Add(orderedFood);

                // Remove Food from the Basket
                var f = _context.BasketFoods.Where(x => x.FoodId == food.FoodId && x.BasketId == basket.Id).FirstOrDefault();

                if (f != null)
                {
                    _context.BasketFoods.Remove(f);
                }
            }

            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public async Task<List<UserOrdersViewModel>> GetUserOrders()
        {
            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }


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


            List<UserOrdersViewModel> orders = new List<UserOrdersViewModel>();

            var userOrders = _context.Orders.Where(x => x.AppUserId == userId).OrderByDescending(x => x.DateCreated).ToList();

            foreach( var order in userOrders) {
                var orderDetails = new UserOrdersViewModel
                {
                    Id = order.Id,
                    OrderAddress = order.OrderAdress,
                    OrderDate = order.DateCreated,
                    TotalAmount = order.OrderAmountDue,
                    Status = order.Status
                };

                orders.Add(orderDetails);
            }

            return orders;
        }

        public async Task<OrderDetailsPageViewModel> GetOrderDetails(int id)
        {
            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;
            var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

            if (userClaim == null)
            {
                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }

            userId = userClaim.Value;

            var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();

            if (user == null)
            {
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }

            List<OrderDetailsViewModel> orderDetails = new List<OrderDetailsViewModel>();

            var orderedFoods = _context.OrderFoods.Include(x => x.Food).Where(x => x.OrderId == id).ToList();

            foreach (var order in orderedFoods)
            {
                var orderedFood = new OrderDetailsViewModel
                {
                    FoodId = order.Food.Id,
                    ImagePath = order.Food.ImagePath,
                    FoodName = order.Food.Name,
                    Quantity = order.Quantity,
                    UnitPrice = order.Price,
                    TotalPrice = order.Price * order.Quantity,
                    Status = order.Status,
                };

                orderDetails.Add(orderedFood);
            }

            var userOrders = _context.Orders.Where(x => x.AppUserId == userId && x.Id == id).FirstOrDefault();

            if(userOrders == null)
            {
                return new OrderDetailsPageViewModel();
            }

            OrderDetailsPageViewModel model = new OrderDetailsPageViewModel
            {
                Foods = orderDetails,
                OrderId = id,
                OrderAddress = userOrders.OrderAdress,
                OrderStatus = userOrders.Status,
                TotalAmount = userOrders.OrderAmountDue,
                OrderDate = userOrders.DateCreated
            };

            return model;
        }

        public async Task<bool> CancelOrder(int id)
        {
            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;
            var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

            if (userClaim == null)
            {
                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }

            userId = userClaim.Value;

            var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();

            if (user == null)
            {
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }

            var order = _context.Orders.Where(x => x.AppUserId == userId && x.Id == id).FirstOrDefault();

            if(order == null)
            {
                return false;
            }

            order.Status = OrderStatus.Cancelled;

            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }
    }
}
