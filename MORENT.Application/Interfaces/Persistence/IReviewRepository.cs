using MORENT.Application.DTOs;
using MORENT.Domain.Entities.Dbo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MORENT.Application.Interfaces.Persistence
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IReadOnlyList<ReviewDto>> GetReviewsByCarIdAsync(Guid CarId);
        Task<double> GetAverageRatingForCarAsync(Guid CarId);
    }
}
