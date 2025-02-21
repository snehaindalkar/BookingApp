using Microsoft.EntityFrameworkCore;

namespace MyBookingApp.MyBookingApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Update connection string as needed
            services.AddDbContext<BookingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
