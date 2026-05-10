using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Security
{
    [Table("Roles", Schema = "security")]
    public class Role : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
