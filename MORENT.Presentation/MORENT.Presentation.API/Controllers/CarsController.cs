using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MORENT.Application.Interfaces.Services;

namespace MORENT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularCars([FromQuery] int count = 4)
        {
            var result = await _carService.GetPopularCarsAsync(count);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedCars([FromQuery] int count = 4)
        {
            var result = await _carService.GetFeaturedCarsAsync(count);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredCars(
            [FromQuery] string? searchTerm,
            [FromQuery] string? carType,
            [FromQuery] int? pickUpLocationId,
            [FromQuery] int? capacity,
            [FromQuery] string? steeringType,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 9)
        {
            var result = await _carService.GetFilteredCarsAsync(
                searchTerm, carType, pickUpLocationId, capacity, steeringType, maxPrice, pageNumber, pageSize);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCarDetails(Guid id)
        {
            var result = await _carService.GetCarDetailsAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}