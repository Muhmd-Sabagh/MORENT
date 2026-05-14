using MORENT.Application.Common;
using MORENT.Application.DTOs;
using MORENT.Domain.Entities.Dbo;

namespace MORENT.Application.Interfaces.Persistence
{
    public interface ICarRepository : IGenericRepository<Car>
    {
        Task<IReadOnlyList<CarDto>> GetPopularCarsAsync(int count);
        Task<IReadOnlyList<CarDto>> GetRecommendedCarsAsync(int count);
        Task<IReadOnlyList<LocationDto>> GetAvailableLocationsAsync();
        Task<PagedResult<CarDto>> GetFilteredCarsAsync(
            string? searchTerm, string? carType, int? pickUpLocationId,
            int? capacity, string? steeringType, decimal? maxPrice,
            DateTime? pickUpDate, DateTime? dropOffDate,
            int pageNumber, int pageSize);
        Task<bool> IsCarAvailableAsync(Guid carId, int pickUpLocationId, DateTime pickUpDate, DateTime dropOffDate);
        Task<CarDto?> GetCarDetailsAsync(Guid carId);
        Task<IReadOnlyList<CarDto>> GetUserFavoriteCarsAsync(Guid userId);
        Task<FavoriteCar?> GetFavoriteAsync(Guid userId, Guid carId);
        Task AddFavoriteAsync(FavoriteCar favorite);
        void RemoveFavorite(FavoriteCar favorite);
    }
}
