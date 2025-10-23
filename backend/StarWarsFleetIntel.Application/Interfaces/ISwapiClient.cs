using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFleetIntel.Application.Common.Wrappers;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Application.Interfaces
{
    public interface ISwapiClient
    {
        Task<Result<Starship>> GetStarshipAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<PaginatedResult<Starship>>> GetStarshipsAsync(int page = 1, CancellationToken cancellationToken = default);
    }
}
