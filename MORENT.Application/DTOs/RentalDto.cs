using System;
using System.Collections.Generic;
using System.Text;

namespace MORENT.Application.DTOs
{
    public class RentalDto
    {
        public Guid Id { get; set; }
        public string CarBrand { get; set; } = string.Empty;
        public string PickUpLocation { get; set; } = string.Empty;
        public string DropOffLocation { get; set; } = string.Empty;
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public string RentalStatus { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public Guid UserId { get; set; }
    }
}
