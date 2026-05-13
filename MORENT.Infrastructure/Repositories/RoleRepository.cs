using Microsoft.EntityFrameworkCore;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Domain.Entities.Security;
using MORENT.Infrastructure.Persistence;

namespace MORENT.Infrastructure.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());
        }
    }
}
