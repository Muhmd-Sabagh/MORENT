using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Services;

namespace MORENT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalRequestDto request)
        {
            // Extract the user ID directly from the JWT Token claims for secure ownership
            var userIdString = User.FindFirstValue("uid");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized("Invalid token claims.");
            }

            var result = await _rentalService.CreateRentalAsync(request, userId);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetRentalDetails(Guid id)
        {
            var userIdString = User.FindFirstValue("uid");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized();
            }

            var result = await _rentalService.GetRentalDetailsAsync(id, userId);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}