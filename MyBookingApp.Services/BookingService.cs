using Microsoft.EntityFrameworkCore;
using MyBookingApp.MyBookingApp.Infrastructure;
using MyBookingApp.MyBookingApp.Infrastructure.DomainEntity;

namespace MyBookingApp.MyBookingApp.Services
{
    public class BookingService : IBookingService
    {
        private readonly BookingDbContext _context;
        private const int MAX_BOOKINGS = 2;

        public BookingService(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<bool> BookItemAsync(Guid memberId, Guid inventoryId)
        {
            // Retrieve member and inventory
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == memberId);
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.Id == inventoryId);

            if (member == null || inventory == null)
                return false;

            // Check constraints
            if (member.BookingCount >= MAX_BOOKINGS || inventory.RemainingCount <= 0)
                return false;

            // Create new booking
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                MemberId = member.Id,
                InventoryId = inventory.Id,
                BookingDate = DateTime.UtcNow
            };

            // Update counts
            member.BookingCount++;
            inventory.RemainingCount--;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelBookingAsync(Guid bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.Inventory)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                return false;

            // Reverse booking counts
            booking.Member.BookingCount--;
            booking.Inventory.RemainingCount++;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
