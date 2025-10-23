using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using StarWarsFleetIntel.Application.Common.Wrappers;
using StarWarsFleetIntel.Domain.Enums;

namespace StarWarsFleetIntel.Application.Features.Starships.Queries.GetStarshipById
{
    public class GetStarshipByIdQuery : IRequest<Result<GetStarshipResponse>>
    {
        public int Id { get; set; }
        public Currency? TargetCurrency { get; set; }
        public List<ModificationType> Modifications { get; set; } = new();
        public bool RunPreFlightChecks { get; set; } = true;
    }
}
