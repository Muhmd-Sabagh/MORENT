using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MORENT.Application.Common;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Domain.Entities.Dbo;
using MORENT.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MORENT.Infrastructure.Repositories
{
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        private readonly IMapper _mapper;
        public CarRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CarDto>> GetPopularCarsAsync(int count)
        {
            return await _dbSet
                .OrderByDescending(c => c.Rentals.Count) // Adjusted for 'Rentals' navigation
                .Take(count)
                .ProjectTo<CarDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<CarDto>> GetFeaturedCarsAsync(int count)
        {
            return await _dbSet
                .OrderByDescending(c => c.AverageRating)
                .Take(count)
                .ProjectTo<CarDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<PagedResult<CarDto>> GetFilteredCarsAsync(
            string? searchTerm, string? carType, int? pickUpLocationId,
            int? capacity, string? steeringType, decimal? maxPrice,
            int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            if (pickUpLocationId.HasValue)
                query = query.Where(c => c.CurrentLocationId == pickUpLocationId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(c => c.Description.Contains(searchTerm) || c.Brand.Contains(searchTerm));

            if (!string.IsNullOrWhiteSpace(carType))
                query = query.Where(c => c.CarType.Name.ToLower() == carType.ToLower());

            if (capacity.HasValue)
                query = query.Where(c => c.Capacity >= capacity.Value);

            if (!string.IsNullOrWhiteSpace(steeringType))
                query = query.Where(c => c.SteeringType.Name.ToLower() == steeringType.ToLower());

            if (maxPrice.HasValue)
                query = query.Where(c => c.PricePerDay <= maxPrice.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<CarDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<CarDto>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<bool> IsCarAvailableAsync(Guid carId, int pickUpLocationId, DateTime pickUpDate, DateTime dropOffDate)
        {
            var carExists = await _dbSet.AnyAsync(c => c.Id == carId && c.CurrentLocationId == pickUpLocationId);
            if (!carExists) return false;

            return !await _context.Set<Rental>()
                .AnyAsync(r => r.CarId == carId
                            && r.RentalStatus.Name != "Cancelled"
                            && r.PickUpDate < dropOffDate
                            && r.DropOffDate > pickUpDate);
        }

        public async Task<CarDto?> GetCarDetailsAsync(Guid carId)
        {
            return await _dbSet
                .Where(c => c.Id == carId)
                .ProjectTo<CarDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
