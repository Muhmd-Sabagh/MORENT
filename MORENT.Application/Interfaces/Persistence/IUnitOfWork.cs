namespace MORENT.Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        ICarRepository Cars { get; }
        IRentalRepository Rentals { get; }
        IReviewRepository Reviews { get; }
        IFavoriteCarRepository FavoriteCars { get; }
        IPromoCodeRepository PromoCodes { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
