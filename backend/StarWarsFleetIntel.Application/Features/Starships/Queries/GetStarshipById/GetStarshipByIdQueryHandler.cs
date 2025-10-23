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
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Application.Features.Starships.Queries.GetStarshipById
{
    public class GetStarshipByIdQueryHandler : IRequestHandler<GetStarshipByIdQuery, Result<GetStarshipResponse>>
    {
        private readonly ISwapiClient _swapiClient;
        private readonly IMapper _mapper;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly IStarshipDecoratorFactory _decoratorFactory;
        private readonly IPreFlightCheckHandler _preFlightCheckHandler;
        private readonly ILogger<GetStarshipByIdQueryHandler> _logger;

        public GetStarshipByIdQueryHandler(
            ISwapiClient swapiClient,
            IMapper mapper,
            ICurrencyConverter currencyConverter,
            IStarshipDecoratorFactory decoratorFactory,
            IPreFlightCheckHandler preFlightCheckHandler,
            ILogger<GetStarshipByIdQueryHandler> logger)
        {
            _swapiClient = swapiClient;
            _mapper = mapper;
            _currencyConverter = currencyConverter;
            _decoratorFactory = decoratorFactory;
            _preFlightCheckHandler = preFlightCheckHandler;
            _logger = logger;
        }

        public async Task<Result<GetStarshipResponse>> Handle(GetStarshipByIdQuery request, CancellationToken cancellationToken)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = Activity.Current?.Id ?? Guid.NewGuid().ToString(),
                ["StarshipId"] = request.Id
            }))
            {
                _logger.LogInformation("Fetching starship with ID {StarshipId}", request.Id);

                var starshipResult = await _swapiClient.GetStarshipAsync(request.Id, cancellationToken);

                if (!starshipResult.Succeeded)
                {
                    _logger.LogWarning("Failed to fetch starship: {Message}", starshipResult.Message);
                    return Result<GetStarshipResponse>.Failure(starshipResult.Message, starshipResult.Errors);
                }

                var starship = starshipResult.Data!;

                // Apply Decorator Pattern - Add modifications
                if (request.Modifications.Any())
                {
                    _logger.LogInformation("Applying {Count} modifications", request.Modifications.Count);
                    foreach (var modification in request.Modifications)
                    {
                        var decorator = _decoratorFactory.CreateDecorator(modification);
                        starship = decorator.Decorate(starship);
                    }
                }

                // Apply Chain of Responsibility - Pre-flight checks
                PreFlightCheckResult? preFlightResult = null;
                if (request.RunPreFlightChecks)
                {
                    _logger.LogInformation("Running pre-flight checks");
                    preFlightResult = await _preFlightCheckHandler.HandleAsync(starship);
                }

                var starshipDto = _mapper.Map<GetStarshipResponse>(starship);

                // Apply Strategy Pattern - Currency conversion
                if (request.TargetCurrency.HasValue && starship.CostInCreditsNumeric.HasValue)
                {
                    _logger.LogInformation("Converting cost to {Currency}", request.TargetCurrency.Value);
                    starshipDto.CostConverted = _currencyConverter.Convert(
                        starship.CostInCreditsNumeric.Value,
                        request.TargetCurrency.Value);
                    starshipDto.Currency = _currencyConverter.GetCurrencySymbol(request.TargetCurrency.Value);
                }

                if (preFlightResult != null)
                {
                    starshipDto.PreFlightCheck = _mapper.Map<PreFlightCheckSummaryDto>(preFlightResult);
                }

                _logger.LogInformation("Successfully processed starship {Name}", starship.Name);
                return Result<GetStarshipResponse>.Success(starshipDto);
            }
        }
    }
}
