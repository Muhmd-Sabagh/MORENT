using AutoMapper;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MORENT.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        private IUserRepository? _users;
        private IRoleRepository? _roles;
        private ICarRepository? _cars;
        private IRentalRepository? _rentals;
        private IReviewRepository? _reviews;
        private IFavoriteCarRepository? _favoriteCars;

        public UnitOfWork(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
        public ICarRepository Cars => _cars ??= new CarRepository(_context, _mapper);
        public IRentalRepository Rentals => _rentals ??= new RentalRepository(_context, _mapper);
        public IReviewRepository Reviews => _reviews ??= new ReviewRepository(_context, _mapper);
        public IFavoriteCarRepository FavoriteCars => _favoriteCars ??= new FavoriteCarRepository(_context, _mapper);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
