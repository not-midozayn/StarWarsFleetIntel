using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFleetIntel.Application.Features.Starships.Queries;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Application.Mapping.StarshipMapping
{
    public partial class StarShipMappingProfile
    {
        public void PreFlightChecksMapping()
        {
            CreateMap<PreFlightCheckResult, PreFlightCheckSummaryDto>()
                    .ForMember(d => d.Passed, opt => opt.MapFrom(s => s.IsPassed));
        }
    }
}
