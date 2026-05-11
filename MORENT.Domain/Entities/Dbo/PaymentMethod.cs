using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("PaymentMethods", Schema = "dbo")]
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
