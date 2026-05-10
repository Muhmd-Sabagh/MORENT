using MORENT.Domain.Entities.Security;
using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("Reviews", Schema = "dbo")]
    public class Review : BaseEntity
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;

        // Foreign Keys
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid CarId { get; set; }
        public Car Car { get; set; } = null!;
    }
}
