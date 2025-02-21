namespace MyBookingApp.MyBookingApp.Api.Contract
{
    public class MemberCsvRecord
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BookingCount { get; set; }
        public DateTime DateJoined { get; set; }
    }
}
