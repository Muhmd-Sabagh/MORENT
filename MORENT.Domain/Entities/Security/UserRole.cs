using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Security
{
    [Table("UserRoles", Schema = "security")]
    public class UserRole
    {
        // Composite Key: UserId + RoleId
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}
