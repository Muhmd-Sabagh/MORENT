using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Domain.Entities.Dbo;
using MORENT.Infrastructure.Persistence;

namespace MORENT.Infrastructure.Repositories
{
    public class FavoriteCarRepository : IFavoriteCarRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FavoriteCarRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CarDto>> GetUserFavoriteCarsAsync(Guid userId)
        {
            return await _context.FavoriteCars
                .Where(f => f.UserId == userId)
                .Select(f => f.Car)
                .ProjectTo<CarDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> IsCarFavoritedByUserAsync(Guid userId, Guid carId)
        {
            return await _context.FavoriteCars.AnyAsync(f => f.UserId == userId && f.CarId == carId);
        }

        public async Task AddFavoriteCarAsync(FavoriteCar favoriteCar) => await _context.FavoriteCars.AddAsync(favoriteCar);
        public void RemoveFavoriteCar(FavoriteCar favoriteCar) => _context.FavoriteCars.Remove(favoriteCar);
    }
}
