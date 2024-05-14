using LutongBahayApp.Interfaces;
using LutongBahayApp.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;

namespace LutongBahayApp.Controllers
{
    public class OrderController(IOrderRepository orderRepository) : Controller
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetUserOrders();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOutFoods([FromBody] CheckOutViewModel checkout)
        {
            bool response = await _orderRepository.CheckOutFoods(checkout);

            if (!response)
            {
                return StatusCode(500);
            }
            return Ok();
        }

        public async Task<IActionResult> Detail(int id)
        {
            var orderDetail = await _orderRepository.GetOrderDetails(id);

            if (orderDetail == null || orderDetail.Foods == null)
                return RedirectToAction("NotFound","Errors");


            return View(orderDetail);
        }

        public async Task<IActionResult> CancelOrder(int id)
        {
            await _orderRepository.CancelOrder(id);

            return RedirectToAction("Index");
        }
    }
}
