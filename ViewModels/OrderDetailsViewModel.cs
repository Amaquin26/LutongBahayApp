namespace LutongBahayApp.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int FoodId { get; set; }
        public string ImagePath { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
