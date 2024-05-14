namespace LutongBahayApp.ViewModels
{
    public class MarketDashboardViewModel
    {
        public string MarketName { get; set; }
        public int MarketId { get; set; }
        public bool MarketIsVerified { get; set; }
        public int TotalProducts { get; set; }
        public DateTime DateCreated { get; set; }
        public int SoldProducts { get; set; }
        public int TotalReviews { get; set; }
        public string MarketAddress { get; set; }
    }
}
