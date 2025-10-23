using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using StarWarsFleetIntel.Application.Common.Wrappers;

namespace StarWarsFleetIntel.Application.Features.Starships.Queries.GetStarshipsPaginated
{
    public class GetStarshipsQuery : IRequest<Result<PaginatedResult<GetStarshipResponse>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
