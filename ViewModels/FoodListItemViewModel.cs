namespace LutongBahayApp.ViewModels
{
    public class FoodListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int MarketId { get; set; }
        public string MarketName { get; set; }
        public decimal Price { get; set; }
        public double Rating { get; set; } = 0;
    }
}
