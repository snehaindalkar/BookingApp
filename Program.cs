using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyBookingApp.MyBookingApp.Infrastructure;
using MyBookingApp.MyBookingApp.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

// Register Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Register Booking Service
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IUploadService, UploadService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyBookingApp API",
        Version = "v1",
        Description = "API for uploading CSV data and managing bookings."
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.Run();
