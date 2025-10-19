using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace EVStation_basedRentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenterProfileController : ControllerBase
    {
        private readonly IRenterProfileService _service;

        public RenterProfileController(IRenterProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var renter = await _service.GetByIdAsync(id);
            return renter == null ? NotFound() : Ok(renter);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RenterProfile profile)
        {
            var created = await _service.CreateAsync(profile);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] RenterProfile profile)
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
        // Renter-Specific Actions
        // -------------------------------
        [HttpPost("{renterId}/update-driver-license")]
        public async Task<IActionResult> UpdateDriverLicense(string renterId, [FromBody] DriverLicenseDto dto)
        {
            var updated = await _service.UpdateDriverLicenseAsync(renterId, dto.Number, dto.ImageUrl);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpPost("{renterId}/update-identity-card")]
        public async Task<IActionResult> UpdateIdentityCard(string renterId, [FromBody] IdentityCardDto dto)
        {
            var updated = await _service.UpdateIdentityCardAsync(renterId, dto.Number, dto.ImageUrl);
            return updated == null ? NotFound() : Ok(updated);
        }
    }

    public record DriverLicenseDto(string Number, string ImageUrl);
    public record IdentityCardDto(string Number, string ImageUrl);
}
