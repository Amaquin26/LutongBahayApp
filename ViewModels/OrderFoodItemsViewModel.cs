namespace LutongBahayApp.ViewModels
{
    public class OrderFoodItemsViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
