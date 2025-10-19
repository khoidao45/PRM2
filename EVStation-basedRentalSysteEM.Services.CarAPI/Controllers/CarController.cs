using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EVStation_basedRentalSystem.Services.CarAPI.Models;
using EVStation_basedRentalSystem.Services.CarAPI.Services.IService;
using EVStation_basedRentalSystem.Services.CarAPI.Models.DTO;
using AutoMapper;

namespace EVStation_basedRentalSystem.Services.CarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IStationService _stationService;
        private readonly IMapper _mapper;

        public CarController(ICarService carService, IStationService stationService, IMapper mapper)
        {
            _carService = carService;
            _stationService = stationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();

            var carDto = _mapper.Map<CarDto>(car);
            carDto.Station = await _stationService.GetStationByIdAsync(car.StationId);

            return Ok(carDto);
        }
        [HttpPut("{carId}/assign/{stationId}")]
        public async Task<IActionResult> AssignCarToStation(int carId, int stationId)
        {
            var car = await _carService.GetCarByIdAsync(carId);
            if (car == null)
                return NotFound("Car not found");

            var station = await _stationService.GetStationByIdAsync(stationId);
            if (station == null)
                return BadRequest("Station not found");

            car.StationId = stationId;
            await _carService.UpdateCarAsync(car);

            return Ok(new
            {
                message = $"Car {carId} successfully assigned to Station {stationId}",
                carId,
                stationId
            });
        }



        [HttpPost]
        public async Task<IActionResult> AddCar([FromBody] Car car)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _carService.AddCarAsync(car);
            return CreatedAtAction(nameof(GetCarById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] Car car)
        {
            if (id != car.Id)
                return BadRequest("Car ID mismatch");

            await _carService.UpdateCarAsync(car);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var result = await _carService.DeleteCarAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpGet("station/{stationId}")]
        public async Task<IActionResult> GetCarsByStation(int stationId)
        {
            var cars = await _carService.GetCarsByStationIdAsync(stationId);
            return Ok(cars);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableCars()
        {
            var cars = await _carService.GetAvailableCarsAsync();
            return Ok(cars);
        }

        [HttpPatch("{id}/state")]
        public async Task<IActionResult> UpdateState(int id, [FromBody] string newState)
        {
            var result = await _carService.UpdateCarStateAsync(id, newState);
            if (!result)
                return NotFound();
            return NoContent();
        }

    }
}
