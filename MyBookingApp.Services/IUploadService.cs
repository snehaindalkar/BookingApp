namespace MyBookingApp.MyBookingApp.Services
{
    public interface IUploadService
    {
        Task<(bool Success, string Message)> ProcessMembersFileAsync(IFormFile file);
        Task<(bool Success, string Message)> ProcessInventoryFileAsync(IFormFile file);
    }
}
