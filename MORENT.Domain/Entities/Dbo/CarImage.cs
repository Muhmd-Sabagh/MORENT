using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("CarImages", Schema = "dbo")]
    public class CarImage : BaseEntity
    {
        public string ImageUrl { get; set; } = "https://placehold.co/400";
        public bool IsMain { get; set; } = false;

        // Foreign Keys
        public Guid CarId { get; set; }
        public Car Car { get; set; } = null!;
    }
}
