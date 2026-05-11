using System;

namespace MORENT.Application.DTOs
{
    public class CreateRentalRequestDto
    {
        public Guid CarId { get; set; }
        public int PickUpLocationId { get; set; }
        public int DropOffLocationId { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public int PaymentMethodId { get; set; }
        public string? PromoCode { get; set; }
    }
}