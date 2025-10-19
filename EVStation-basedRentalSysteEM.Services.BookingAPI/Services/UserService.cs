using EVStation_basedRentalSysteEM.Services.BookingAPI.Models.Dto;
using EVStation_basedRentalSystem.Services.BookingAPI.Models.Dto;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(string userId);
}

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserService> _logger;

    public UserService(HttpClient httpClient, ILogger<UserService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<UserDto?> GetUserByIdAsync(string userId)
    {
        try
        {
            // Use string ID for AuthAPI
            var response = await _httpClient.GetFromJsonAsync<UserDto>($"api/Auth/{userId}");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user from AuthAPI with ID {UserId}", userId);
            return null;
        }
    }
}
