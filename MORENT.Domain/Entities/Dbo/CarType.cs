using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("CarTypes", Schema = "dbo")]
    public class CarType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
