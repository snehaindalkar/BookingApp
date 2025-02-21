using Microsoft.AspNetCore.Mvc;
using MyBookingApp.MyBookingApp.Services;
using System.Text;

namespace MyBookingApp.MyBookingApp.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }
        public class FileUploadRequest
        {
            public IFormFile File { get; set; }
        }
        // Endpoint for uploading members CSV
        [HttpPost("members")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadMembers([FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file provided.");

            // Process file contents
            using var stream = request.File.OpenReadStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            // Option 1: Read all text and process manually
            string fileContent = await reader.ReadToEndAsync();

            // Option 2: Use CSV Helper to parse the file (uncomment below if using CsvHelper)
            /*
            stream.Position = 0; // Reset stream position
            using var csvReader = new CsvReader(new StreamReader(stream, Encoding.UTF8), CultureInfo.InvariantCulture);
            var records = csvReader.GetRecords<MemberCsvRecord>(); // Make sure you have a proper mapping/model
            var memberList = new List<Member>();
            foreach (var record in records)
            {
                // Map CSV record to your Member entity
                memberList.Add(new Member
                {
                    Id = Guid.NewGuid(),
                    FirstName = record.Name,
                    LastName = record.Surname,
                    BookingCount = record.BookingCount,
                    DateJoined = record.DateJoined
                });
            }
            */

            // For demo purposes, we call our service to handle file processing (you can pass fileContent or stream)
            var result = await _uploadService.ProcessMembersFileAsync(request.File);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        // Endpoint for uploading inventory CSV
        [HttpPost("inventory")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadInventory([FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file provided.");

            // Process file contents (similar logic)
            var result = await _uploadService.ProcessInventoryFileAsync(request.File);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }

    // Example CSV record mapping for members (if using CSV Helper)
    public class MemberCsvRecord
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BookingCount { get; set; }
        public DateTime DateJoined { get; set; }
    }
}