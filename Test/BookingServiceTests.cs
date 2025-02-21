using Microsoft.EntityFrameworkCore;
using MyBookingApp.MyBookingApp.Infrastructure.DomainEntity;
using MyBookingApp.MyBookingApp.Infrastructure;
using MyBookingApp.MyBookingApp.Services;

namespace MyBookingAppTests.Test
{
    [TestClass]
    public class BookingServiceTests
    {
        private BookingDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new BookingDbContext(options);
        }

        [TestMethod]
        public async Task BookItemAsync_ShouldSucceed_WhenMemberAndInventoryValid()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var member = new Member
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                BookingCount = 0,
                DateJoined = DateTime.UtcNow
            };
            var inventory = new Inventory
            {
                Id = Guid.NewGuid(),
                Title = "Test Inventory",
                Description = "Test description",
                RemainingCount = 5,
                ExpirationDate = DateTime.UtcNow.AddDays(10)
            };

            await context.Members.AddAsync(member);
            await context.Inventories.AddAsync(inventory);
            await context.SaveChangesAsync();

            var service = new BookingService(context);

            // Act
            var result = await service.BookItemAsync(member.Id, inventory.Id);

            // Assert
            Assert.IsTrue(result, "Booking should succeed.");
            var updatedMember = await context.Members.FindAsync(member.Id);
            Assert.AreEqual(1, updatedMember.BookingCount, "Member booking count should be incremented.");
            var updatedInventory = await context.Inventories.FindAsync(inventory.Id);
            Assert.AreEqual(4, updatedInventory.RemainingCount, "Inventory remaining count should be decremented.");
        }

        [TestMethod]
        public async Task BookItemAsync_ShouldFail_WhenMemberReachedMaxBookings()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var member = new Member
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                BookingCount = 2, // Already at MAX_BOOKINGS = 2
                DateJoined = DateTime.UtcNow
            };
            var inventory = new Inventory
            {
                Id = Guid.NewGuid(),
                Title = "Test Inventory",
                Description = "Test description",
                RemainingCount = 5,
                ExpirationDate = DateTime.UtcNow.AddDays(10)
            };

            await context.Members.AddAsync(member);
            await context.Inventories.AddAsync(inventory);
            await context.SaveChangesAsync();

            var service = new BookingService(context);

            // Act
            var result = await service.BookItemAsync(member.Id, inventory.Id);

            // Assert
            Assert.IsFalse(result, "Booking should fail because member reached maximum bookings.");
        }

        [TestMethod]
        public async Task CancelBookingAsync_ShouldSucceed_WhenBookingExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var member = new Member
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                BookingCount = 1,
                DateJoined = DateTime.UtcNow
            };
            var inventory = new Inventory
            {
                Id = Guid.NewGuid(),
                Title = "Test Inventory",
                Description = "Test description",
                RemainingCount = 4,
                ExpirationDate = DateTime.UtcNow.AddDays(10)
            };

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                MemberId = member.Id,
                InventoryId = inventory.Id,
                BookingDate = DateTime.UtcNow,
                Member = member,
                Inventory = inventory
            };

            await context.Members.AddAsync(member);
            await context.Inventories.AddAsync(inventory);
            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();

            var service = new BookingService(context);

            // Act
            var result = await service.CancelBookingAsync(booking.Id);

            // Assert
            Assert.IsTrue(result, "Cancellation should succeed.");
            var updatedMember = await context.Members.FindAsync(member.Id);
            Assert.AreEqual(0, updatedMember.BookingCount, "Member booking count should be decremented.");
            var updatedInventory = await context.Inventories.FindAsync(inventory.Id);
            Assert.AreEqual(5, updatedInventory.RemainingCount, "Inventory remaining count should be incremented.");
        }
    }
}
