namespace MyBookingApp.MyBookingApp.Api.Contract
{
    public class InventoryCsvRecord
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int RemainingCount { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
