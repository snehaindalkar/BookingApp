namespace MyBookingApp.MyBookingApp.Infrastructure.DomainEntity
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public Member? Member { get; set; }
        public Guid InventoryId { get; set; }
        public Inventory? Inventory { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
