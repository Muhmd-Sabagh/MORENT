using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("FuelTypes", Schema = "dbo")]
    public class FuelType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
