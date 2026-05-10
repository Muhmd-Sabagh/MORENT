using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("SteeringTypes", Schema = "dbo")]
    public class SteeringType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
