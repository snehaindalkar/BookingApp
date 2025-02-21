using Microsoft.AspNetCore.Mvc;
using Moq;
using MyBookingApp.MyBookingApp.Api.Contract;
using MyBookingApp.MyBookingApp.Api.Controller;
using MyBookingApp.MyBookingApp.Services;

namespace MyBookingApp.Test.Tests
{
    [TestClass]
    public class BookingControllerTests
    {
        private Mock<IBookingService> _bookingServiceMock;
        private BookingController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _controller = new BookingController(_bookingServiceMock.Object);
        }

        [TestMethod]
        public async Task Book_ReturnsOk_WhenBookingSucceeds()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var inventoryId = Guid.NewGuid();
            _bookingServiceMock
                .Setup(s => s.BookItemAsync(memberId, inventoryId))
                .ReturnsAsync(true);

            var request = new BookRequest
            {
                MemberId = memberId,
                InventoryId = inventoryId
            };

            // Act
            var result = await _controller.Book(request);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult");
            Assert.AreEqual("Booking successful.", okResult.Value);
        }

        [TestMethod]
        public async Task Book_ReturnsBadRequest_WhenBookingFails()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var inventoryId = Guid.NewGuid();
            _bookingServiceMock
                .Setup(s => s.BookItemAsync(memberId, inventoryId))
                .ReturnsAsync(false);

            var request = new BookRequest
            {
                MemberId = memberId,
                InventoryId = inventoryId
            };

            // Act
            var result = await _controller.Book(request);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult, "Expected BadRequestObjectResult");
            Assert.AreEqual("Booking failed: Check booking limits or inventory availability.", badRequestResult.Value);
        }

        [TestMethod]
        public async Task Cancel_ReturnsOk_WhenCancellationSucceeds()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            _bookingServiceMock
                .Setup(s => s.CancelBookingAsync(bookingId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Cancel(bookingId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult");
            Assert.AreEqual("Booking cancelled.", okResult.Value);
        }

        [TestMethod]
        public async Task Cancel_ReturnsNotFound_WhenCancellationFails()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            _bookingServiceMock
                .Setup(s => s.CancelBookingAsync(bookingId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Cancel(bookingId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult, "Expected NotFoundObjectResult");
            Assert.AreEqual("Booking not found or cancellation failed.", notFoundResult.Value);
        }
    }
}