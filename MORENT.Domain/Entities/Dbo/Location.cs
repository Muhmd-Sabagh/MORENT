using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("Locations", Schema = "dbo")]
    public class Location : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
