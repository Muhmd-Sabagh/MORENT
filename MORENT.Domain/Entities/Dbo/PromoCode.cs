using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("PromoCodes", Schema = "dbo")]
    public class PromoCode : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
