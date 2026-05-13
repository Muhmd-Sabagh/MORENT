using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MORENT.Application.Common;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Auth;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Domain.Entities.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MORENT.Infrastructure.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork uow, IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            _uow = uow;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
        {
            // 1. Checks if user exists and is active
            var user = await _uow.Users.GetByUsernameAsync(request.Username);

            if (user == null || !user.IsActive)
            {
                return Result<AuthResponse>.Failure("Invalid Username or Invalid Password, or inactive account.");
            }

            // 2. Verify Password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Result<AuthResponse>.Failure("Invalid Username or Invalid Password.");
            }

            // 3. Generate Tokens
            var response = await GenerateAuthenticationResultForUserAsync(user);
            return Result<AuthResponse>.Success(response);
        }

        public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
        {
            // 1. Check if username is unique
            if (!await _uow.Users.IsUsernameUniqueAsync(request.Username))
            {
                return Result<AuthResponse>.Failure($"Username '{request.Username}' is already exist.");
            }

            // 2. Check if email is unique
            if (!string.IsNullOrEmpty(request.Email) && !await _uow.Users.IsEmailUniqueAsync(request.Email))
            {
                return Result<AuthResponse>.Failure($"Email '{request.Email}' is already used.");
            }

            // 3. Map and Hash Password
            var user = _mapper.Map<User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 4. Assign Default Role ('Client') and Adding user
            var defaultRole = await _uow.Roles.GetByNameAsync("Client");
            if (defaultRole == null)
            {
                return Result<AuthResponse>.Failure("Default role 'Client' not found. Please seed roles properly.");
            }
            user.RoleId = defaultRole.Id;
            user.Role = defaultRole;

            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();

            // 5. Generate Tokens
            var response = await GenerateAuthenticationResultForUserAsync(user);
            return Result<AuthResponse>.Success(response);
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(string token)
        {
            var user = await _uow.Users.GetByRefreshTokenAsync(token);

            if (user == null)
            {
                return Result<AuthResponse>.Failure("Invalid refresh token.");
            }

            var existingToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == token);

            if (existingToken == null || existingToken.IsRevoked || existingToken.ExpiresAt <= DateTime.UtcNow)
            {
                return Result<AuthResponse>.Failure("Invalid or expired session. Please log in again.");
            }

            // Revoke the old token
            existingToken.IsRevoked = true;

            // Generate new tokens
            var response = await GenerateAuthenticationResultForUserAsync(user);

            return Result<AuthResponse>.Success(response);
        }

        private async Task<AuthResponse> GenerateAuthenticationResultForUserAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("uid", user.Id.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Generate Refresh Token
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            await _uow.Users.AddRefreshTokenAsync(refreshToken);
            await _uow.SaveChangesAsync();

            var response = _mapper.Map<AuthResponse>(user);
            response.Token = tokenHandler.WriteToken(token);
            response.RefreshToken = refreshToken.Token;
            response.RefreshTokenExpiry = refreshToken.ExpiresAt;

            return response;
        }
    }
}