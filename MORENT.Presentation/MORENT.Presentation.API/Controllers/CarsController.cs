using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MORENT.Application.Interfaces.Services;
using System.Security.Claims;

namespace MORENT.Presentation.API.Controllers
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

        [AllowAnonymous]
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularCars([FromQuery] int count = 4)
        {
            var result = await _carService.GetPopularCarsAsync(count);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommendedCars([FromQuery] int count = 4)
        {
            var result = await _carService.GetRecommendedCarsAsync(count);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("locations")]
        public async Task<IActionResult> GetAvailableLocations()
        {
            var result = await _carService.GetAvailableLocationsAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetFilteredCars(
            [FromQuery] string? searchTerm,
            [FromQuery] string? carType,
            [FromQuery] int? pickUpLocationId,
            [FromQuery] int? capacity,
            [FromQuery] string? steeringType,
            [FromQuery] decimal? maxPrice,
            [FromQuery] DateTime? pickUpDate, // <-- NEW PARAMETERS
            [FromQuery] DateTime? dropOffDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 9)
        {
            var result = await _carService.GetFilteredCarsAsync(
                searchTerm, carType, pickUpLocationId, capacity, steeringType, maxPrice,
                pickUpDate, dropOffDate, pageNumber, pageSize);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCarDetails(Guid id)
        {
            var result = await _carService.GetCarDetailsAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var userIdString = User.FindFirstValue("uid");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized("Invalid token claims.");
            }

            var result = await _carService.GetUserFavoriteCarsAsync(userId);
            return Ok(result);
        }

        [HttpPost("{carId:guid}/favorite")]
        public async Task<IActionResult> ToggleFavorite(Guid carId)
        {
            var userIdString = User.FindFirstValue("uid");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized("Invalid token claims.");
            }

            var result = await _carService.ToggleFavoriteAsync(userId, carId);
            return Ok(result);
        }
    }
}