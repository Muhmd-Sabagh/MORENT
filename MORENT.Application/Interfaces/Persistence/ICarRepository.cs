using MORENT.Application.DTOs;
using MORENT.Domain.Entities.Dbo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MORENT.Application.Interfaces.Persistence
{
    public interface ICarRepository : IGenericRepository<Car>
    {
        Task<IReadOnlyList<CarDto>> GetPopularCarsAsync(int count);
        Task<IReadOnlyList<CarDto>> GetFeaturedCarsAsync(int count);
        Task<IReadOnlyList<CarDto>> GetFilteredCarsAsync(
            string? searchTerm, string? carType, Guid? pickUpLocationId,
            int? capacity, string? steeringType, decimal? maxPrice);
        Task<bool> IsCarAvailableAsync(Guid carId, Guid pickUpLocationId, DateTime pickUpDate, DateTime dropOffDate);
        Task<CarDto?> GetCarDetailsAsync(Guid carId);
    }
}
