using MORENT.Domain.Entities.Security;

namespace MORENT.Application.Interfaces.Persistence
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<bool> IsUsernameUniqueAsync(string username);
        Task<bool> IsEmailUniqueAsync(string email);
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
    }
}
