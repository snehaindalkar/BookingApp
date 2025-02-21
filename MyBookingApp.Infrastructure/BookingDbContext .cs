using Microsoft.EntityFrameworkCore;
using MyBookingApp.MyBookingApp.Infrastructure.DomainEntity;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MyBookingApp.MyBookingApp.Infrastructure
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity mappings if needed
            base.OnModelCreating(modelBuilder);
        }
    }
}

