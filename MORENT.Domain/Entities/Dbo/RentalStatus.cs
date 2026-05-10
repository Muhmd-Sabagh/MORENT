using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("RentalStatus", Schema = "dbo")]
    public class RentalStatus : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
