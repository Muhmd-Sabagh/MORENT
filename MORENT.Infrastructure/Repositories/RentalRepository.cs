using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Domain.Entities.Dbo;
using MORENT.Infrastructure.Persistence;

namespace MORENT.Infrastructure.Repositories
{
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
    {
        private readonly IMapper _mapper;

        public RentalRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<RentalDto?> GetRentalWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Where(r => r.Id == id)
                .ProjectTo<RentalDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<PaymentMethodDto>> GetAvailabePaymentMethodsAsync()
        {
            return await _context.PaymentMethods
                .AsNoTracking()
                .OrderBy(pm => pm.Name)
                .Select(pm => new PaymentMethodDto { Id = pm.Id, Name = pm.Name })
                .ToListAsync();
        }

        public async Task<int> GetTotalRentalsCountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<IReadOnlyList<CarTypeStatDto>> GetTopCarsByRentalAsync(int count)
        {
            return await _dbSet
                .GroupBy(r => r.Car.CarType)
                .Select(g => new CarTypeStatDto
                {
                    Type = g.Key.ToString(),
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<RecentTransactionDto>> GetRecentTransactionsAsync(int count)
        {
            return await _dbSet
                .OrderByDescending(r => r.CreatedAt)
                .Take(count)
                .ProjectTo<RecentTransactionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ActiveRentalDto?> GetLatestActiveRentalAsync()
        {
            return await _dbSet
                .Where(r => r.RentalStatusId == 1) // 1 = Confirmed/Active
                .OrderByDescending(r => r.CreatedAt)
                .ProjectTo<ActiveRentalDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}