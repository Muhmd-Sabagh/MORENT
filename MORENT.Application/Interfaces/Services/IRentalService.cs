using MORENT.Application.Common;
using MORENT.Application.DTOs;

namespace MORENT.Application.Interfaces.Services
{
    public interface IRentalService
    {
        Task<Result<Guid>> CreateRentalAsync(CreateRentalRequestDto request, Guid userId);
        Task<Result<RentalDto>> GetRentalDetailsAsync(Guid rentalId, Guid userId);
    }
}