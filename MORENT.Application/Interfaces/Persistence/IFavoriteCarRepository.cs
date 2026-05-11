using MORENT.Application.DTOs;
using MORENT.Domain.Entities.Dbo;

namespace MORENT.Application.Interfaces.Persistence
{
    public interface IFavoriteCarRepository
    {
        Task<IReadOnlyList<CarDto>> GetUserFavoriteCarsAsync(Guid userId);
        Task<bool> IsCarFavoritedByUserAsync(Guid userId, Guid carId);
        Task AddFavoriteCarAsync(FavoriteCar favoriteCar);
        void RemoveFavoriteCar(FavoriteCar favoriteCar);
    }
}
