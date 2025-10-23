using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using StarWarsFleetIntel.Application.Features.Starships.Queries;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Application.Mapping.StarshipMapping
{
    public partial class StarShipMappingProfile : Profile
    {
        public StarShipMappingProfile()
        {
            StarshipMapping();
            PreFlightChecksMapping();
        }
    }
}
