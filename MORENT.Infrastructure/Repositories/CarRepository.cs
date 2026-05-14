using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MORENT.Application.Common;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Domain.Entities.Dbo;
using MORENT.Infrastructure.Persistence;

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
            // Top rented cars first. If there are no rentals, fallback to newest cars
            return await _dbSet
                .OrderByDescending(c => c.Rentals.Count)
                .ThenByDescending(c => c.CreatedAt)
                .Take(count)
                .ProjectTo<CarDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<CarDto>> GetRecommendedCarsAsync(int count)
        {
            var today = DateTime.UtcNow;

            // Highest rated cars that are AVAILABLE right now
            return await _dbSet
                .Where(c => !c.Rentals.Any(r => r.RentalStatus.Name != "Cancelled"
                                             && r.PickUpDate <= today
                                             && r.DropOffDate >= today))
                .OrderByDescending(c => c.AverageRating)
                .Take(count)
                .ProjectTo<CarDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

            public async Task<IReadOnlyList<LocationDto>> GetAvailableLocationsAsync()
            {
                return await _context.Locations
                .AsNoTracking()
                .OrderBy(l => l.Name)
                .Select(l => new LocationDto { Id = l.Id, Name = l.Name })
                .ToListAsync();
            }

        public async Task<PagedResult<CarDto>> GetFilteredCarsAsync(
            string? searchTerm, string? carType, int? pickUpLocationId,
            int? capacity, string? steeringType, decimal? maxPrice,
            DateTime? pickUpDate, DateTime? dropOffDate,
            int pageNumber = 1, int pageSize = 9)
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

            if (pickUpDate.HasValue && dropOffDate.HasValue)
            {
                query = query.Where(c => !c.Rentals.Any(r =>
                    r.RentalStatus.Name != "Cancelled" &&
                    r.PickUpDate < dropOffDate.Value &&
                    r.DropOffDate > pickUpDate.Value));
            }

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

        // Favorites
        public async Task<IReadOnlyList<CarDto>> GetUserFavoriteCarsAsync(Guid userId)
        {
            return await _context.FavoriteCars
                .AsNoTracking()
                .Where(f => f.UserId == userId)
                .Select(f => f.Car)
                .ProjectTo<CarDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<FavoriteCar?> GetFavoriteAsync(Guid userId, Guid carId)
        {
            return await _context.FavoriteCars
                .FirstOrDefaultAsync(f => f.UserId == userId && f.CarId == carId);
        }

        public async Task AddFavoriteAsync(FavoriteCar favorite)
        {
            await _context.FavoriteCars.AddAsync(favorite);
        }

        public void RemoveFavorite(FavoriteCar favorite)
        {
            _context.FavoriteCars.Remove(favorite);
        }
    }
}
