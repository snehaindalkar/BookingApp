using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyBookingApp.MyBookingApp.Api.Controller;
using MyBookingApp.MyBookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyBookingApp.MyBookingApp.Api.Controller.UploadController;

namespace MyBookingAppTests.Test
{
    [TestClass]
    public class UploadControllerTests
    {
        private Mock<IUploadService> _uploadServiceMock;
        private UploadController _controller;

        [TestInitialize]
        public void Setup()
        {
            _uploadServiceMock = new Mock<IUploadService>();
            _controller = new UploadController(_uploadServiceMock.Object);
        }

        [TestMethod]
        public async Task UploadMembers_ReturnsBadRequest_WhenFileIsNull()
        {
            // Arrange
            var request = new FileUploadRequest { File = null };

            // Act
            var result = await _controller.UploadMembers(request);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("No file provided.", badRequestResult.Value);
        }

        [TestMethod]
        public async Task UploadMembers_ReturnsOk_WhenServiceReturnsSuccess()
        {
            // Arrange: create a fake IFormFile (not reading content in this test)
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(100);
            var request = new FileUploadRequest { File = fileMock.Object };

            _uploadServiceMock
                .Setup(s => s.ProcessMembersFileAsync(request.File))
                .ReturnsAsync((true, "Members uploaded successfully."));

            // Act
            var result = await _controller.UploadMembers(request);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Members uploaded successfully.", okResult.Value);
        }

        [TestMethod]
        public async Task UploadInventory_ReturnsBadRequest_WhenServiceReturnsFailure()
        {
            // Arrange: create a fake IFormFile.
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(100);
            var request = new FileUploadRequest { File = fileMock.Object };

            _uploadServiceMock
                .Setup(s => s.ProcessInventoryFileAsync(request.File))
                .ReturnsAsync((false, "Error processing inventory file."));

            // Act
            var result = await _controller.UploadInventory(request);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Error processing inventory file.", badRequestResult.Value);
        }
    }
}