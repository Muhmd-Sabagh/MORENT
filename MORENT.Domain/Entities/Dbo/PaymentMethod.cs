using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("PaymentMethods", Schema = "dbo")]
    public class PaymentMethod : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
