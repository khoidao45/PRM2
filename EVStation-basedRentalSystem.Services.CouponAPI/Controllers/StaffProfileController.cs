using EVStation_basedRentalSystem.Services.AuthAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace EVStation_basedRentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffProfileController : ControllerBase
    {
        private readonly IStaffProfileService _service;

        public StaffProfileController(IStaffProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var staff = await _service.GetByIdAsync(id);
            return staff == null ? NotFound() : Ok(staff);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StaffProfile profile)
        {
            var created = await _service.CreateAsync(profile);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] StaffProfile profile)
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
        // Staff-Specific Actions
        // -------------------------------
        [HttpPost("{staffId}/assign-shift")]
        public async Task<IActionResult> AssignShift(string staffId, [FromBody] string workShift)
        {
            var updated = await _service.AssignShiftAsync(staffId, workShift);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpPost("{staffId}/assign-department")]
        public async Task<IActionResult> AssignDepartment(string staffId, [FromBody] string department)
        {
            var updated = await _service.AssignDepartmentAsync(staffId, department);
            return updated == null ? NotFound() : Ok(updated);
        }
    }
}
