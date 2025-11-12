using Microsoft.AspNetCore.Mvc;
using CarShareBLL.Services;
using CarShareBLL.DTOs;

namespace CarSharePL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        /// <summary>
        /// Get all available cars (public access)
        /// </summary>
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetAvailableCars()
        {
            var cars = await _carService.GetAvailableCarsAsync();
            return Ok(cars);
        }

        /// <summary>
        /// Get car by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCarById(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            
            if (car == null)
            {
                return NotFound(new { message = "Car not found" });
            }

            return Ok(car);
        }

        /// <summary>
        /// Search cars by keyword
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CarDto>>> SearchCars([FromQuery] string q)
        {
            var cars = await _carService.SearchCarsAsync(q);
            return Ok(cars);
        }

        /// <summary>
        /// Filter cars with multiple criteria
        /// </summary>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<CarDto>>> FilterCars([FromBody] CarSearchFilterDto filterDto)
        {
            var cars = await _carService.FilterCarsAsync(filterDto);
            return Ok(cars);
        }

        /// <summary>
        /// Create a new car post (Car Owner only)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CarResponseDto>> CreateCar([FromBody] CreateCarDto createCarDto)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return Unauthorized(new { message = "Please login to create a car post" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new CarResponseDto
                {
                    Success = false,
                    Message = "Invalid data provided"
                });
            }

            var result = await _carService.CreateCarAsync(userId.Value, createCarDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetCarById), new { id = result.Car.Id }, result);
        }

        /// <summary>
        /// Update car post (Owner only)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CarResponseDto>> UpdateCar(int id, [FromBody] UpdateCarDto updateCarDto)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return Unauthorized(new { message = "Please login" });
            }

            if (id != updateCarDto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new CarResponseDto
                {
                    Success = false,
                    Message = "Invalid data provided"
                });
            }

            var result = await _carService.UpdateCarAsync(userId.Value, updateCarDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Delete car post (Owner only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<CarResponseDto>> DeleteCar(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return Unauthorized(new { message = "Please login" });
            }

            var result = await _carService.DeleteCarAsync(id, userId.Value);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get my cars (Owner only)
        /// </summary>
        [HttpGet("my-cars")]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetMyCars()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return Unauthorized(new { message = "Please login" });
            }

            var cars = await _carService.GetCarsByOwnerIdAsync(userId.Value);
            return Ok(cars);
        }

        /// <summary>
        /// Mark car as rented (Owner only)
        /// </summary>
        [HttpPatch("{id}/rent")]
        public async Task<ActionResult<CarResponseDto>> MarkAsRented(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return Unauthorized(new { message = "Please login" });
            }

            var result = await _carService.MarkAsRentedAsync(id, userId.Value);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Mark car as available (Owner only)
        /// </summary>
        [HttpPatch("{id}/available")]
        public async Task<ActionResult<CarResponseDto>> MarkAsAvailable(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return Unauthorized(new { message = "Please login" });
            }

            var result = await _carService.MarkAsAvailableAsync(id, userId.Value);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Approve car post (Admin only)
        /// </summary>
        [HttpPatch("{id}/approve")]
        public async Task<ActionResult<CarResponseDto>> ApproveCar(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (!userId.HasValue || userRole != "Admin")
            {
                return Unauthorized(new { message = "Admin access required" });
            }

            var result = await _carService.ApproveCarAsync(id, userId.Value);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Reject car post (Admin only)
        /// </summary>
        [HttpPatch("{id}/reject")]
        public async Task<ActionResult<CarResponseDto>> RejectCar(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (!userId.HasValue || userRole != "Admin")
            {
                return Unauthorized(new { message = "Admin access required" });
            }

            var result = await _carService.RejectCarAsync(id, userId.Value);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}