using LutongBahayApp.Data;
using LutongBahayApp.Interfaces;
using LutongBahayApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LutongBahayApp.Data.Enum;
using LutongBahayApp.Models;

namespace LutongBahayApp.Repository
{
    public class AdminRepository(ApplicationDbContext context) : IAdminRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<UserOrdersViewModel>> GetOnDeliveryOrders()
        {
            List<UserOrdersViewModel> orders = new List<UserOrdersViewModel>();

            var userOrders = _context.Orders.Where(x => x.Status == OrderStatus.OnDelivery).OrderByDescending(x => x.DateCreated).ToList();

            foreach (var order in userOrders)
            {
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

        public async Task SetMarketDelivery(int flag, int orderId)
        {
            var order = await _context.Orders.Include(x => x.OrderFood).Where(x => x.Id == orderId).FirstOrDefaultAsync();

            if (order == null)
            {
                return;
            }

            if (flag == 0)
            {
                order.Status = OrderStatus.Failed;
                await _context.SaveChangesAsync();
                return;
            }
            
            var orderFoods = order.OrderFood.ToList();

            order.Status = OrderStatus.Success;

            foreach (var orderFood in orderFoods)
            {
                var food = await _context.Foods.Where(x => x.Id == orderFood.FoodId).FirstOrDefaultAsync();

                food.Sales += orderFood.Price * orderFood.Quantity;

                //update sold food metrics
                var soldMetric = await _context.SoldFoodMetrics.Where(x => x.FoodId == food.Id && x.DateSold.Date == DateTime.Today.Date).FirstOrDefaultAsync();

                if (soldMetric != null)
                {
                    soldMetric.SoldCount += orderFood.Quantity;
                }
                else
                {
                    var newSoldMetric = new SoldFoodMetric
                    {
                        FoodId = food.Id,
                        DateSold = DateTime.Today.Date,
                        SoldCount = orderFood.Quantity
                    };

                    _context.SoldFoodMetrics.Add(newSoldMetric);
                }

                //update sold market metrics
                // get markets involved
                var marketId = food.MarketId;

                var marketMetric = await _context.SoldMarketMetrics.Where(x => x.MarketId == marketId).FirstOrDefaultAsync();

                if (marketMetric != null)
                {
                    marketMetric.SoldCount += orderFood.Quantity;
                    marketMetric.DateSold = DateTime.Today.Date;
                    marketMetric.Sales += orderFood.Quantity * orderFood.Price;
                }
                else
                {
                    var newMarketMetric = new SoldMarketMetrics
                    {
                        MarketId = marketId,
                        DateSold = DateTime.Today.Date,
                        SoldCount = orderFood.Quantity,
                        Sales = orderFood.Quantity * orderFood.Price
                    };

                    _context.SoldMarketMetrics.Add(newMarketMetric);
                }
            }

            await _context.SaveChangesAsync();
        }
    }  
}
