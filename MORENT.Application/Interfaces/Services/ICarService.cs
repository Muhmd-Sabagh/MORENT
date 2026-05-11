using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MORENT.Application.Common;
using MORENT.Application.DTOs;

namespace MORENT.Application.Interfaces.Services
{
    public interface ICarService
    {
        Task<Result<IReadOnlyList<CarDto>>> GetPopularCarsAsync(int count = 4);
        Task<Result<IReadOnlyList<CarDto>>> GetFeaturedCarsAsync(int count = 4);
        Task<Result<PagedResult<CarDto>>> GetFilteredCarsAsync(
            string? searchTerm, string? carType, int? pickUpLocationId,
            int? capacity, string? steeringType, decimal? maxPrice,
            int pageNumber = 1, int pageSize = 9);
        Task<Result<CarDto>> GetCarDetailsAsync(Guid carId);
    }
}