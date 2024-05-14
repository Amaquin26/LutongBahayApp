using LutongBahayApp.Models;
using LutongBahayApp.ViewModels;
using LutongBahayApp.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;

namespace LutongBahayApp.Interfaces
{
    public interface IBasketRepository
    {
        public Task<List<BasketItemViewModel>> GetBasketItems();
        public Task<bool> AddToBasket(int foodId, int quantity);
        public Task<bool> UpdateQuantity(int foodId, int quantity);
        public Task<bool> RemoveFromBasket(int id);
    }
}
