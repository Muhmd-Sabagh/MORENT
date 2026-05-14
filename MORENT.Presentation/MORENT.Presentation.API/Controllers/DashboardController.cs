using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MORENT.Application.Interfaces.Services;

namespace MORENT.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Secured
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var result = await _dashboardService.GetDashboardDataAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}