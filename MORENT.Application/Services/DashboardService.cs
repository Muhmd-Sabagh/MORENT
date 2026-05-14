using MORENT.Application.Common;
using MORENT.Application.DTOs;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Application.Interfaces.Services;

namespace MORENT.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _uow;

        public DashboardService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<DashboardDataDto>> GetDashboardDataAsync()
        {
            var totalRentals = await _uow.Rentals.GetTotalRentalsCountAsync();
            var topCarsGrouping = await _uow.Rentals.GetTopCarsByRentalAsync(5);
            var recentTransactions = await _uow.Rentals.GetRecentTransactionsAsync(5);
            var activeRental = await _uow.Rentals.GetLatestActiveRentalAsync();

            var colors = new[] { "var(--primary-900)", "var(--primary-500)", "var(--primary-400)", "var(--primary-300)", "var(--primary-200)" };
            var topCars = topCarsGrouping.Select((c, index) => new CarStatDto
            {
                Type = c.Type,
                Count = c.Count,
                ColorHex = colors[index % colors.Length]
            }).ToList();

            var data = new DashboardDataDto
            {
                TotalRentals = totalRentals,
                TopCars = topCars,
                RecentTransactions = recentTransactions.ToList(),
                ActiveRental = activeRental
            };

            return Result<DashboardDataDto>.Success(data);
        }
    }
}