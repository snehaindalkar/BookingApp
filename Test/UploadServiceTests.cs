using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyBookingApp.MyBookingApp.Infrastructure;
using MyBookingApp.MyBookingApp.Services;
using System.Text;

namespace MyBookingAppTests.Test
{
    [TestClass]
    public class UploadServiceTests
    {
        private BookingDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new BookingDbContext(options);
        }

        [TestMethod]
        public async Task ProcessMembersFileAsync_ReturnsSuccess_WhenFileIsValid()
        {
            // Arrange: Create a CSV string that matches your MemberCsvRecord mapping.
            // Adjust headers and data as needed.
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Name,Surname,BookingCount,DateJoined");
            csvContent.AppendLine("John,Doe,1,2024-01-01T12:00:00");
            csvContent.AppendLine("Jane,Smith,0,2024-01-02T13:00:00");

            // Create a MemoryStream from the CSV string.
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent.ToString()));

            // Simulate an IFormFile. (The parameters here are for testing only.)
            IFormFile formFile = new FormFile(stream, 0, stream.Length, "file", "members.csv");

            using var context = GetInMemoryDbContext();
            var service = new UploadService(context);

            // Act
            var result = await service.ProcessMembersFileAsync(formFile);

            // Assert
            Assert.IsTrue(result.Success, "Processing members file should succeed.");
            Assert.AreEqual("Members uploaded successfully.", result.Message);
            // Verify that two members have been added.
            var count = await context.Members.CountAsync();
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public async Task ProcessInventoryFileAsync_ReturnsSuccess_WhenFileIsValid()
        {
            // Arrange: Create a CSV string that matches your InventoryCsvRecord mapping.
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Title,Description,RemainingCount,ExpirationDate");
            csvContent.AppendLine("TestItem,Description,5,2030-12-31");

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent.ToString()));
            IFormFile formFile = new FormFile(stream, 0, stream.Length, "file", "inventory.csv");

            using var context = GetInMemoryDbContext();
            var service = new UploadService(context);

            // Act
            var result = await service.ProcessInventoryFileAsync(formFile);

            // Assert
            Assert.IsTrue(result.Success, "Processing inventory file should succeed.");
            Assert.AreEqual("Inventory uploaded successfully.", result.Message);
            var count = await context.Inventories.CountAsync();
            Assert.AreEqual(1, count);
        }
    }
}