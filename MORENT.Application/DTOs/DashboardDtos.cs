using System;
using System.Collections.Generic;

namespace MORENT.Application.DTOs
{
    public class DashboardDataDto
    {
        public ActiveRentalDto? ActiveRental { get; set; }
        public int TotalRentals { get; set; }
        public List<CarStatDto> TopCars { get; set; } = new();
        public List<RecentTransactionDto> RecentTransactions { get; set; } = new();
    }

    public class ActiveRentalDto
    {
        public string CarName { get; set; } = string.Empty;
        public string CarType { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string PickUpLocation { get; set; } = string.Empty;
        public DateTime PickUpDate { get; set; }
        public string DropOffLocation { get; set; } = string.Empty;
        public DateTime DropOffDate { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CarTypeStatDto
    {
        public string Type { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class CarStatDto : CarTypeStatDto
    {
        public string ColorHex { get; set; } = string.Empty;
    }

    public class RecentTransactionDto
    {
        public string Id { get; set; } = string.Empty;
        public string CarName { get; set; } = string.Empty;
        public string CarType { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}