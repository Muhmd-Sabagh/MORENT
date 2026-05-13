using MORENT.Domain.Entities.Security;

namespace MORENT.Application.Interfaces.Persistence
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string roleName);
    }
}
