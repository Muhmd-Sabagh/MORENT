using MORENT.Domain.Entities.Security;
using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("FavoriteCars", Schema = "dbo")]
    public class FavoriteCar
    {
        // Composite Key: UserId + CarId
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid CarId { get; set; }
        public Car Car { get; set; } = null!;
    }
}
