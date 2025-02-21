using CsvHelper;
using MyBookingApp.MyBookingApp.Infrastructure.DomainEntity;
using MyBookingApp.MyBookingApp.Infrastructure;
using System.Globalization;
using MyBookingApp.MyBookingApp.Api.Controller;

namespace MyBookingApp.MyBookingApp.Services
{
    public class UploadService : IUploadService
    {
        private readonly BookingDbContext _context;

        public UploadService(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> ProcessMembersFileAsync(IFormFile file)
        {
            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<MemberCsvRecord>(); // Use the mapping class

                var members = new List<Member>();
                foreach (var record in records)
                {
                    members.Add(new Member
                    {
                        Id = Guid.NewGuid(),
                        FirstName = record.Name,
                        LastName = record.Surname,
                        BookingCount = record.BookingCount,
                        DateJoined = record.DateJoined
                    });
                }
                await _context.Members.AddRangeAsync(members);
                await _context.SaveChangesAsync();
                return (true, "Members uploaded successfully.");
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return (false, $"Error processing members file: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> ProcessInventoryFileAsync(IFormFile file)
        {
            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<InventoryCsvRecord>();

                var inventoryItems = new List<Inventory>();
                foreach (var record in records)
                {
                    inventoryItems.Add(new Inventory
                    {
                        Id = Guid.NewGuid(),
                        Title = record.Title,
                        Description = record.Description,
                        RemainingCount = record.RemainingCount,
                        ExpirationDate = record.ExpirationDate
                    });
                }
                await _context.Inventories.AddRangeAsync(inventoryItems);
                await _context.SaveChangesAsync();
                return (true, "Inventory uploaded successfully.");
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return (false, $"Error processing inventory file: {ex.Message}");
            }
        }
    }

    // CSV mapping model for inventory records
    public class InventoryCsvRecord
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int RemainingCount { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
