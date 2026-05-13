using MORENT.Application.DTOs;
using MORENT.Domain.Entities.Dbo;

namespace MORENT.Application.Interfaces.Persistence
{
    public interface IRentalRepository : IGenericRepository<Rental>
    {
        Task<RentalDto?> GetRentalWithDetailsAsync(Guid id);
    }
}
