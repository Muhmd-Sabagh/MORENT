using MORENT.Domain.Entities.Dbo;

namespace MORENT.Application.Interfaces.Persistence
{
    public interface IPromoCodeRepository : IGenericRepository<PromoCode>
    {
        Task<PromoCode?> GetByCodeAsync(string code);
    }
}