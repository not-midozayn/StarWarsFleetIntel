using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarWarsFleetIntel.Application.Common.Wrappers;
using StarWarsFleetIntel.Application.Features.Starships.Queries;
using StarWarsFleetIntel.Application.Features.Starships.Queries.GetStarshipById;
using StarWarsFleetIntel.Application.Features.Starships.Queries.GetStarshipsPaginated;
using StarWarsFleetIntel.Domain.Enums;

namespace StarWarsFleetIntel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StarshipsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StarshipsController> _logger;

        public StarshipsController(
            IMediator mediator,
            ILogger<StarshipsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Result<GetStarshipResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<GetStarshipResponse>>> GetStarship(
            int id,
            [FromQuery] Currency? currency = null,
            [FromQuery] string? modifications = null,
            [FromQuery] bool runPreFlightChecks = true)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = HttpContext.Items["CorrelationId"]?.ToString() ?? "N/A",
                ["Endpoint"] = "GetStarship"
            }))
            {
                _logger.LogInformation("Processing request for starship {StarshipId}", id);

                var modificationsList = new List<ModificationType>();
                if (!string.IsNullOrWhiteSpace(modifications))
                {
                    modificationsList = modifications
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(m => Enum.Parse<ModificationType>(m.Trim(), true))
                        .ToList();
                }

                var query = new GetStarshipByIdQuery
                {
                    Id = id,
                    TargetCurrency = currency,
                    Modifications = modificationsList,
                    RunPreFlightChecks = runPreFlightChecks
                };

                var result = await _mediator.Send(query);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Request failed: {Message}", result.Message);
                    return NotFound(result);
                }

                return Ok(result);
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedResult<GetStarshipResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<PaginatedResult<GetStarshipResponse>>>> GetStarships(
            [FromQuery] int page = 1,
            [FromQuery] Currency? currency = null)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = HttpContext.Items["CorrelationId"]?.ToString() ?? "N/A",
                ["Endpoint"] = "GetStarships"
            }))
            {
                _logger.LogInformation("Fetching starships page {Page}", page);

                var query = new GetStarshipsQuery
                {
                    Page = page,
                    TargetCurrency = currency
                };

                var result = await _mediator.Send(query);
                return Ok(result);
            }
        }
    }
}
