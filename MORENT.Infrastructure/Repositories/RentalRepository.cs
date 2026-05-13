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
    }
}
