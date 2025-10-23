using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using StarWarsFleetIntel.Application.Common.Wrappers;
using StarWarsFleetIntel.Application.Interfaces;

namespace StarWarsFleetIntel.Application.Features.Starships.Queries.GetStarshipsPaginated
{
    public class GetStarshipsQueryHandler : IRequestHandler<GetStarshipsQuery, Result<PaginatedResult<GetStarshipResponse>>>
    {
        private readonly ISwapiClient _swapiClient;
        private readonly IMapper _mapper;
        private readonly ILogger<GetStarshipsQueryHandler> _logger;

        public GetStarshipsQueryHandler(
            ISwapiClient swapiClient,
            IMapper mapper,
            ILogger<GetStarshipsQueryHandler> logger)
        {
            _swapiClient = swapiClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PaginatedResult<GetStarshipResponse>>> Handle(GetStarshipsQuery request, CancellationToken cancellationToken)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = Activity.Current?.Id ?? Guid.NewGuid().ToString(),
                ["Page"] = request.Page
            }))
            {
                _logger.LogInformation("Fetching starships page {Page}", request.Page);

                var result = await _swapiClient.GetStarshipsAsync(request.Page, cancellationToken);

                if (!result.Succeeded)
                {
                    return Result<PaginatedResult<GetStarshipResponse>>.Failure(result.Message);
                }

                var dtos = result.Data!.Items.Select(_mapper.Map<GetStarshipResponse>).ToList();

                var paginatedResult = new PaginatedResult<GetStarshipResponse>
                {
                    Items = dtos,
                    PageNumber = result.Data.PageNumber,
                    PageSize = result.Data.PageSize,
                    TotalCount = result.Data.TotalCount
                };

                return Result<PaginatedResult<GetStarshipResponse>>.Success(paginatedResult);
            }
        }
    }
}
