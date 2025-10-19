using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EVStation_basedRentalSystem.Services.StationAPI.Models;
using EVStation_basedRentalSystem.Services.StationAPI.Services.IService;

namespace EVStation_basedRentalSystem.Services.StationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationService _stationService;

        public StationController(IStationService stationService)
        {
            _stationService = stationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStations() =>
            Ok(await _stationService.GetAllStationsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStationById(int id)
        {
            var station = await _stationService.GetStationByIdAsync(id);
            if (station == null) return NotFound();
            return Ok(station);
        }

        [HttpPost]
        public async Task<IActionResult> AddStation([FromBody] Station station)
        {
            var created = await _stationService.AddStationAsync(station);
            return CreatedAtAction(nameof(GetStationById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStation(int id, [FromBody] Station station)
        {
            if (id != station.Id) return BadRequest("Station ID mismatch");
            await _stationService.UpdateStationAsync(station);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStation(int id)
        {
            var result = await _stationService.DeleteStationAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveStations() =>
            Ok(await _stationService.GetActiveStationsAsync());
    }
}
