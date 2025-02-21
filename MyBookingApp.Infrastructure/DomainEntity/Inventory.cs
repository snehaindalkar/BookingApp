namespace MyBookingApp.MyBookingApp.Infrastructure.DomainEntity
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int RemainingCount { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
