using System.ComponentModel.DataAnnotations.Schema;

namespace MORENT.Domain.Entities.Dbo
{
    [Table("Car", Schema = "dbo")]
    public class Car : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public string FuelCapacity { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerDay { get; set; }
        public decimal? Discount { get; set; }
        public decimal AverageRating { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Foreign Keys
        public Guid CurrentLocationId { get; set; }
        public Location CurrentLocation { get; set; } = null!;

        public Guid CarTypeId { get; set; }
        public CarType CarType { get; set; } = null!;

        public Guid FuelTypeId { get; set; }
        public FuelType FuelType { get; set; } = null!;

        public Guid SteeringTypeId { get; set; }
        public SteeringType SteeringType { get; set; } = null!;

        // Navigation Properties
        public ICollection<CarImage> CarImages { get; set; } = new List<CarImage>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public ICollection<FavoriteCar> FavoriteCars { get; set; } = new List<FavoriteCar>();
    }
}
