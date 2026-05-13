using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Domain.Entities.Dbo;
using MORENT.Infrastructure.Persistence;

namespace MORENT.Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly IMapper _mapper;

        public ReviewRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ReviewDto>> GetReviewsByCarIdAsync(Guid carId)
        {
            return await _dbSet
                .Where(r => r.CarId == carId)
                .OrderByDescending(r => r.CreatedAt)
                .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingForCarAsync(Guid carId)
        {
            var reviews = await _dbSet.Where(r => r.CarId == carId).Select(r => r.Rating).ToListAsync();
            return reviews.Any() ? reviews.Average() : 0;
        }
    }
}
