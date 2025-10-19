using EVStation_basedRentalSystem.Services.CarAPI.Data;
using EVStation_basedRentalSystem.Services.CarAPI.Services;
using EVStation_basedRentalSystem.Services.CarAPI.Services.IService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(Program));
// Add services to the container.
builder.Services.AddDbContext<CarDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IStationService, StationService>(c =>
{
    c.BaseAddress = new Uri("https://localhost:7004"); // replace with your StationAPI base URL
});
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
