using EVStation_basedRentalSystem.Services.UserAPI.Models;
using EVStation_basedRentalSystem.Services.UserAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace EVStation_basedRentalSystem.Services.UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _staffService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var staff = await _staffService.GetByIdAsync(id);
            if (staff == null) return NotFound();
            return Ok(staff);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var staff = await _staffService.GetByEmailAsync(email);
            if (staff == null) return NotFound();
            return Ok(staff);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyself()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var staff = await _staffService.GetMyselfAsync(token);
            if (staff == null) return NotFound();
            return Ok(staff);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Staff staff)
        {
            var created = await _staffService.CreateAsync(staff);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Staff staff)
        {
            if (id != staff.Id) return BadRequest();

            var updated = await _staffService.UpdateAsync(staff);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _staffService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/assign-shift")]
        public async Task<IActionResult> AssignShift(string id, [FromQuery] string shift)
        {
            var staff = await _staffService.AssignShiftAsync(id, shift);
            if (staff == null) return NotFound();
            return Ok(staff);
        }

        [HttpPut("{id}/assign-position")]
        public async Task<IActionResult> AssignPosition(string id, [FromQuery] string position)
        {
            var staff = await _staffService.AssignPositionAsync(id, position);
            if (staff == null) return NotFound();
            return Ok(staff);
        }

        [HttpPut("{id}/assign-department")]
        public async Task<IActionResult> AssignDepartment(string id, [FromQuery] string department)
        {
            var staff = await _staffService.AssignDepartmentAsync(id, department);
            if (staff == null) return NotFound();
            return Ok(staff);
        }
    }
}
