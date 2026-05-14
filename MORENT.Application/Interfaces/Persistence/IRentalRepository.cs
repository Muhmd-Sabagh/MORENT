using MORENT.Application.DTOs;
using MORENT.Domain.Entities.Dbo;

namespace MORENT.Application.Interfaces.Persistence
{
    public interface IRentalRepository : IGenericRepository<Rental>
    {
        Task<RentalDto?> GetRentalWithDetailsAsync(Guid id);
        Task<IReadOnlyList<PaymentMethodDto>> GetAvailabePaymentMethodsAsync();
        Task<int> GetTotalRentalsCountAsync();
        Task<IReadOnlyList<CarTypeStatDto>> GetTopCarsByRentalAsync(int count);
        Task<IReadOnlyList<RecentTransactionDto>> GetRecentTransactionsAsync(int count);
        Task<ActiveRentalDto?> GetLatestActiveRentalAsync();
    }
}
