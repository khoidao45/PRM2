using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using EVStation_basedRentalSystem.Services.AuthAPI.Services.IService.Profile;
using Microsoft.AspNetCore.Mvc;

namespace EVStation_basedRentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminProfileController : ControllerBase
    {
        private readonly IAdminProfileService _service;

        public AdminProfileController(IAdminProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var admin = await _service.GetByIdAsync(id);
            return admin == null ? NotFound() : Ok(admin);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdminProfile profile)
        {
            var created = await _service.CreateAsync(profile);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] AdminProfile profile)
        {
            if (id != profile.Id) return BadRequest();
            var updated = await _service.UpdateAsync(profile);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        // -------------------------------
        // Role-Specific Actions
        // -------------------------------
        [HttpPost("{adminId}/approve/{userId}")]
        public async Task<IActionResult> ApproveUser(string adminId, string userId)
        {
            var success = await _service.ApproveUserAsync(adminId, userId);
            return success ? Ok() : NotFound();
        }

        [HttpPost("{adminId}/assign-station")]
        public async Task<IActionResult> AssignStation(string adminId, [FromBody] string stationName)
        {
            var success = await _service.AssignStationAsync(adminId, stationName);
            return success ? Ok() : NotFound();
        }
    }
}
