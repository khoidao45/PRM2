using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EVStation_basedRentalSystem.Services.UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET: api/profile/me
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            var user = await _profileService.GetMyProfileAsync(token);
            if (user == null) return NotFound("User not found");

            return Ok(user);
        }

        // PUT: api/profile/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] ApplicationUser updateRequest)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            var user = await _profileService.UpdateMyProfileAsync(token, updateRequest);
            if (user == null) return NotFound("User not found");

            return Ok(user);
        }

        // PUT: api/profile/image
        [HttpPut("image")]
        public async Task<IActionResult> UpdateProfileImage([FromBody] string imageUrl)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            var user = await _profileService.UpdateMyProfileImageAsync(token, imageUrl);
            if (user == null) return NotFound("User not found");

            return Ok(user);
        }
    }
}
