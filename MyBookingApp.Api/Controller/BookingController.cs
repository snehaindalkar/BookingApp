using Microsoft.AspNetCore.Mvc;
using MyBookingApp.MyBookingApp.Services;
using MyBookingApp.MyBookingApp.Api.Contract;

namespace MyBookingApp.MyBookingApp.Api.Controller
{
    [ApiController]
    [Route("api")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("book")]
        public async Task<IActionResult> Book([FromBody] BookRequest request)
        {
            var success = await _bookingService.BookItemAsync(request.MemberId, request.InventoryId);
            if (!success)
                return BadRequest("Booking failed: Either booking limit reached or inventory unavailable.");
            return Ok("Booking successful.");
        }

        [HttpDelete("cancel/{bookingId}")]
        public async Task<IActionResult> Cancel(Guid bookingId)
        {
            var success = await _bookingService.CancelBookingAsync(bookingId);
            if (!success)
                return NotFound("Booking not found or cancellation failed.");
            return Ok("Booking cancelled.");
        }
    }

    
}

