using MORENT.Domain.Entities.Security;
using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("Rentals", Schema = "dbo")]
    public class Rental : BaseEntity
    {
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }

        // Foreign Keys
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid CarId { get; set; }
        public Car Car { get; set; } = null!;

        public Guid PickUpLocationId { get; set; }
        public Location PickUpLocation { get; set; } = null!;

        public Guid DropOffLocationId { get; set; }
        public Location DropOffLocation { get; set; } = null!;

        public Guid PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = null!;

        public Guid RentalStatusId { get; set; }
        public RentalStatus RentalStatus { get; set; } = null!;

        public Guid? PromoCodeId { get; set; }
        public PromoCode? PromoCode { get; set; }
    }
}
