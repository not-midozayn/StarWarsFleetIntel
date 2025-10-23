using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFleetIntel.Application.Interfaces;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Infrastructure.PreFlightChecks
{
    public abstract class BasePreFlightCheckHandler : IPreFlightCheckHandler
    {
        private IPreFlightCheckHandler? _nextHandler;

        public IPreFlightCheckHandler SetNext(IPreFlightCheckHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public async Task<PreFlightCheckResult> HandleAsync(Starship starship)
        {
            var result = await CheckAsync(starship);

            if (_nextHandler != null)
            {
                var nextResult = await _nextHandler.HandleAsync(starship);
                result.Warnings.AddRange(nextResult.Warnings);
                result.Errors.AddRange(nextResult.Errors);
                result.IsPassed = result.IsPassed && nextResult.IsPassed;
            }

            return result;
        }

        protected abstract Task<PreFlightCheckResult> CheckAsync(Starship starship);
    }
}
