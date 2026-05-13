using Microsoft.EntityFrameworkCore;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Domain.Entities.Dbo;
using MORENT.Infrastructure.Persistence;

namespace MORENT.Infrastructure.Repositories
{
    public class PromoCodeRepository : GenericRepository<PromoCode>, IPromoCodeRepository
    {
        public PromoCodeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PromoCode?> GetByCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Code.ToLower() == code.ToLower());
        }
    }
}