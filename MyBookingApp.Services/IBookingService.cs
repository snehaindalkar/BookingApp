namespace MyBookingApp.MyBookingApp.Services
{
    public interface IBookingService
    {
        Task<bool> BookItemAsync(Guid memberId, Guid inventoryId);
        Task<bool> CancelBookingAsync(Guid bookingId);
    }
}
