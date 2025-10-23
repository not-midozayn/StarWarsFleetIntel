using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Infrastructure.PreFlightChecks
{
    public class HyperdriveCheckHandler : BasePreFlightCheckHandler
    {
        protected override Task<PreFlightCheckResult> CheckAsync(Starship starship)
        {
            var result = new PreFlightCheckResult { CheckName = "Hyperdrive Check", IsPassed = true };

            if (decimal.TryParse(starship.HyperdriveRating, out var rating))
            {
                if (rating > 2.0m)
                {
                    result.Warnings.Add($"Slow hyperdrive rating: {rating}");
                }
            }
            else
            {
                result.Errors.Add("Hyperdrive rating invalid or unknown");
                result.IsPassed = false;
            }

            return Task.FromResult(result);
        }
    }
}
