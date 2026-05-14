using MORENT.Application.Common;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Application.Interfaces.Services;
using MORENT.Domain.Entities.Dbo;

namespace MORENT.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _uow;

        public CarService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<IReadOnlyList<CarDto>>> GetPopularCarsAsync(int count = 4)
        {
            var cars = await _uow.Cars.GetPopularCarsAsync(count);
            return Result<IReadOnlyList<CarDto>>.Success(cars);
        }

        public async Task<Result<IReadOnlyList<CarDto>>> GetRecommendedCarsAsync(int count = 4)
        {
            var cars = await _uow.Cars.GetRecommendedCarsAsync(count);
            return Result<IReadOnlyList<CarDto>>.Success(cars);
        }

        public async Task<Result<IReadOnlyList<LocationDto>>> GetAvailableLocationsAsync()
        {
            var locations = await _uow.Cars.GetAvailableLocationsAsync();
            return Result<IReadOnlyList<LocationDto>>.Success(locations);
        }

        public async Task<Result<PagedResult<CarDto>>> GetFilteredCarsAsync(
            string? searchTerm, string? carType, int? pickUpLocationId,
            int? capacity, string? steeringType, decimal? maxPrice,
            DateTime? pickUpDate, DateTime? dropOffDate,
            int pageNumber = 1, int pageSize = 9)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 9;
            if (pageSize > 45) pageSize = 45;

            var cars = await _uow.Cars.GetFilteredCarsAsync(searchTerm, carType, pickUpLocationId,
                capacity, steeringType, maxPrice, pickUpDate, dropOffDate, pageNumber, pageSize);
            return Result<PagedResult<CarDto>>.Success(cars);
        }

        public async Task<Result<CarDto>> GetCarDetailsAsync(Guid carId)
        {
            var car = await _uow.Cars.GetCarDetailsAsync(carId);
            if (car == null)
            {
                return Result<CarDto>.Failure("Car not found.");
            }

            return Result<CarDto>.Success(car);
        }

        public async Task<Result<IReadOnlyList<CarDto>>> GetUserFavoriteCarsAsync(Guid userId)
        {
            var cars = await _uow.Cars.GetUserFavoriteCarsAsync(userId);
            return Result<IReadOnlyList<CarDto>>.Success(cars);
        }

        public async Task<Result<bool>> ToggleFavoriteAsync(Guid userId, Guid carId)
        {
            var existingFavorite = await _uow.Cars.GetFavoriteAsync(userId, carId);

            if (existingFavorite != null)
            {
                // Unfavorite
                _uow.Cars.RemoveFavorite(existingFavorite);
                await _uow.SaveChangesAsync();

                return Result<bool>.Success(false, "Removed from favorites");
            }

            // Favorite
            var favorite = new FavoriteCar
            {
                UserId = userId,
                CarId = carId
            };

            await _uow.Cars.AddFavoriteAsync(favorite);
            await _uow.SaveChangesAsync();

            return Result<bool>.Success(true, "Added to favorites");
        }
    }
}