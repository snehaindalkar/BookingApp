namespace MyBookingApp.MyBookingApp.Api.Contract
{
    public class BookRequest
    {
        public Guid MemberId { get; set; }
        public Guid InventoryId { get; set; }
    }
}
