namespace LutongBahayApp.ViewModels.Post
{
    public class CheckOutViewModel
    {
        public List<CheckOutFoodViewModel> FoodBaskets { get; set; }
        public int TotalDue { get; set; }
        public string? OrderAddress { get; set; }
    }
}
