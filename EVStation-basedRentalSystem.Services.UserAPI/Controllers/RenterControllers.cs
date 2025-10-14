using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Models.DTO;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace EVStation_basedRentalSystem.Services.UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenterController : ControllerBase
    {
        private readonly IRenterService _renterService;

        public RenterController(IRenterService renterService)
        {
            _renterService = renterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _renterService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var renter = await _renterService.GetByIdAsync(id);
            if (renter == null) return NotFound();
            return Ok(renter);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var renter = await _renterService.GetByEmailAsync(email);
            if (renter == null) return NotFound();
            return Ok(renter);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyself()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var renter = await _renterService.GetMyselfAsync(token);
            if (renter == null) return NotFound();
            return Ok(renter);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Renter renter)
        {
            var created = await _renterService.CreateAsync(renter);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Renter renter)
        {
            if (id != renter.Id) return BadRequest();

            var updated = await _renterService.UpdateAsync(renter);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpPost("{id}/upload-driver-license")]
        public async Task<IActionResult> UploadDriverLicense(string id, [FromBody] DriverLicenseUrlDto request)
        {
            if (string.IsNullOrEmpty(request.LicenseImageUrl))
                return BadRequest("License image URL is required.");

            var renter = await _renterService.UpdateDriverLicenseUrlAsync(id, request.LicenseImageUrl);
            if (renter == null) return NotFound();

            return Ok(renter);
        }

    }
}
