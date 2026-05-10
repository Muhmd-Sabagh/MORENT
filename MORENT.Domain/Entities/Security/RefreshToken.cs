using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Security
{
    [Table("RefreshTokens", Schema = "security")]
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;

        // Foreign Keys
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
