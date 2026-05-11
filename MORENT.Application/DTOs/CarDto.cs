using System;
using System.Collections.Generic;
using System.Text;

namespace MORENT.Application.DTOs
{
    public class CarDto
    {
        public Guid Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CarType { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public string SteeringType { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerDay { get; set; }
        public decimal? Discount { get; set; }
        public string MainImageUrl { get; set; } = string.Empty;
        public decimal AverageRating { get; set; }
    }
}
