using MORENT.Application.Common;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Application.Interfaces.Services;

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

        public async Task<Result<IReadOnlyList<CarDto>>> GetFeaturedCarsAsync(int count = 4)
        {
            var cars = await _uow.Cars.GetFeaturedCarsAsync(count);
            return Result<IReadOnlyList<CarDto>>.Success(cars);
        }

        public async Task<Result<PagedResult<CarDto>>> GetFilteredCarsAsync(
            string? searchTerm, string? carType, int? pickUpLocationId,
            int? capacity, string? steeringType, decimal? maxPrice,
            int pageNumber = 1, int pageSize = 9)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 9;
            if (pageSize > 45) pageSize = 45;

            var cars = await _uow.Cars.GetFilteredCarsAsync(searchTerm, carType, pickUpLocationId,
                capacity, steeringType, maxPrice, pageNumber, pageSize);
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
    }
}