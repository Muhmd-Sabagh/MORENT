using MORENT.Application.DTOs;
using MORENT.Domain.Entities.Dbo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MORENT.Application.Interfaces.Persistence
{
    public interface IRentalRepository : IGenericRepository<Rental>
    {
        Task<RentalDto?> GetRentalWithDetailsAsync(Guid id);
    }
}
