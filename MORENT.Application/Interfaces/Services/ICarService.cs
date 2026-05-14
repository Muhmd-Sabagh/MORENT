using MORENT.Application.Common;
using MORENT.Application.DTOs;

namespace MORENT.Application.Interfaces.Services
{
    public interface ICarService
    {
        Task<Result<IReadOnlyList<CarDto>>> GetPopularCarsAsync(int count = 4);
        Task<Result<IReadOnlyList<CarDto>>> GetRecommendedCarsAsync(int count = 4);
        Task<Result<IReadOnlyList<LocationDto>>> GetAvailableLocationsAsync();
        Task<Result<PagedResult<CarDto>>> GetFilteredCarsAsync(
            string? searchTerm, string? carType, int? pickUpLocationId,
            int? capacity, string? steeringType, decimal? maxPrice,
            DateTime? pickUpDate, DateTime? dropOffDate,
            int pageNumber = 1, int pageSize = 9);
        Task<Result<CarDto>> GetCarDetailsAsync(Guid carId);
        Task<Result<IReadOnlyList<CarDto>>> GetUserFavoriteCarsAsync(Guid userId);
        Task<Result<bool>> ToggleFavoriteAsync(Guid userId, Guid carId);
    }
}