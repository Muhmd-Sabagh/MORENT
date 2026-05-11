using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("SteeringTypes", Schema = "dbo")]
    public class SteeringType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
