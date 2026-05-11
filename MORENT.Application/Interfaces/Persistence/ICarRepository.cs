using MORENT.Application.Common;
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
        Task<PagedResult<CarDto>> GetFilteredCarsAsync(
            string? searchTerm, string? carType, int? pickUpLocationId,
            int? capacity, string? steeringType, decimal? maxPrice,
            int pageNumber, int pageSize);
        Task<bool> IsCarAvailableAsync(Guid carId, int pickUpLocationId, DateTime pickUpDate, DateTime dropOffDate);
        Task<CarDto?> GetCarDetailsAsync(Guid carId);
    }
}
