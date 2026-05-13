using Microsoft.AspNetCore.Mvc;
using MORENT.Application.Common;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Auth;

namespace MORENT.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }

            SetRefreshTokenCookie(result.DataObject?.RefreshToken, result.DataObject?.RefreshTokenExpiry);

            return Ok(FormatAuthResponse(result));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            SetRefreshTokenCookie(result.DataObject?.RefreshToken, result.DataObject?.RefreshTokenExpiry);

            return Ok(FormatAuthResponse(result));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            // Read the secure cookie instead of the request body
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(Result<object>.Failure("Refresh token is missing."));
            }

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }

            // Update the cookie with the newly generated refresh token
            SetRefreshTokenCookie(result.DataObject?.RefreshToken, result.DataObject?.RefreshTokenExpiry);

            return Ok(FormatAuthResponse(result));
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Clear the cookie on logout
            Response.Cookies.Delete("refreshToken");
            return Ok(Result<object>.Success(null, "Logged out successfully."));
        }

        // --- Helper Methods ---

        private void SetRefreshTokenCookie(string? token, DateTime? expiry)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensure your dev environment uses HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = expiry ?? DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", token ?? string.Empty, cookieOptions);
        }

        private Result<object> FormatAuthResponse(Result<AuthResponse> result)
        {
            // Strips the refresh token out of the JSON response payload
            return Result<object>.Success(
                new
                {
                    result.DataObject?.Id,
                    result.DataObject?.FirstName,
                    result.DataObject?.LastName,
                    result.DataObject?.Email,
                    result.DataObject?.Username,
                    result.DataObject?.Token,
                    result.DataObject?.Role
                },
                result.Message
            );
        }
    }
}