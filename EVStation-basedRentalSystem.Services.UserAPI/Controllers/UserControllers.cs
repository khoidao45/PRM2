using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Models.DTO;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace EVStation_basedRentalSystem.Services.UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("sync")]
        public async Task<IActionResult> SyncUser([FromBody] UserDto user)
        {
            await _userService.CreateUserAsync(user);
            return Ok(new { Message = "User synced successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            var created = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] User user)
        {
            if (id != user.Id) return BadRequest();

            var updated = await _userService.UpdateAsync(user);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _userService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyself()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var user = await _userService.GetMyselfAsync(token);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("{id}/upload-profile")]
        public async Task<IActionResult> UploadProfileImage(string id, [FromBody] ProfileImageDto request)
        {
            if (string.IsNullOrEmpty(request.ImageUrl))
                return BadRequest("Image URL is required.");

            var user = await _userService.UpdateProfileImageUrlAsync(id, request.ImageUrl);
            if (user == null) return NotFound();

            return Ok(user);
        }

    }
}
