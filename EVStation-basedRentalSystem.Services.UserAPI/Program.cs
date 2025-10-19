using EVStation_basedRentalSystem.Services.UserAPI.Clients;
using EVStation_basedRentalSystem.Services.UserAPI.Data;
using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Services;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Database
// --------------------
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --------------------
// AutoMapper
// --------------------
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// --------------------
// HttpClient for AuthService
// --------------------
builder.Services.AddHttpClient<AuthServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthService:BaseUrl"]);
});

// --------------------
// JWT
// --------------------
var jwtOptions = builder.Configuration.GetSection("JwtOptions");
var secret = jwtOptions.GetValue<string>("Secret");
var issuer = jwtOptions.GetValue<string>("Issuer");
var audience = jwtOptions.GetValue<string>("Audience");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});




// --------------------
// Controllers & Swagger
// --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Version = "v1" });
    c.OperationFilter<SwaggerFileOperationFilter>();
});

// --------------------
// DI for services
// --------------------
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IRenterService, RenterService>();

builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

var app = builder.Build();


// --------------------
// Middleware
// --------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// --------------------
// Swagger filter for IFormFile
// --------------------
public class SwaggerFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.RequestBody == null)
            operation.RequestBody = new OpenApiRequestBody();

        var fileParams = context.MethodInfo.GetParameters()
            .Where(p => typeof(IFormFile).IsAssignableFrom(p.ParameterType)
                     || typeof(IEnumerable<IFormFile>).IsAssignableFrom(p.ParameterType));

        if (!fileParams.Any()) return;

        operation.RequestBody.Content.Clear();
        operation.RequestBody.Content.Add("multipart/form-data", new OpenApiMediaType
        {
            Schema = new OpenApiSchema
            {
                Type = "object",
                Properties = fileParams.ToDictionary(
                    p => p.Name,
                    p => new OpenApiSchema { Type = "string", Format = "binary" })
            }
        });
    }
}
