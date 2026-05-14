using MORENT.Application.Common;
using MORENT.Application.DTOs;

namespace MORENT.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<Result<DashboardDataDto>> GetDashboardDataAsync();
    }
}
