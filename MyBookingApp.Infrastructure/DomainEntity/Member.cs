namespace MyBookingApp.MyBookingApp.Infrastructure.DomainEntity
{
    public class Member
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BookingCount { get; set; }
        public DateTime DateJoined { get; set; }
    }
}
