using MORENT.Domain.Entities.Dbo;
using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Security
{
    [Table("Users", Schema = "security")]
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<FavoriteCar> FavoriteCars { get; set; } = new List<FavoriteCar>();
    }
}
