using MORENT.Application.Common;
using MORENT.Application.DTOs;

namespace MORENT.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
        Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request);
        Task<Result<AuthResponse>> RefreshTokenAsync(string token);
    }
}