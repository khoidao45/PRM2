using EVStation_basedRentalSystem.Services.PaymentAPI.Data;

using EVStation_basedRendtalSystem.Services.PaymentAPI.Services.IService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Add DbContext
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PaymentDbConnection")));

// 2️⃣ HttpClient for Booking API
builder.Services.AddHttpClient<IBookingService, BookingService>();

// 3️⃣ Register PayOSService as singleton (it handles PayOS internally)
builder.Services.AddSingleton<PayOSService>();

// 4️⃣ Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5️⃣ Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
