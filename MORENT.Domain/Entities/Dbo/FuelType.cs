using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("FuelTypes", Schema = "dbo")]
    public class FuelType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
