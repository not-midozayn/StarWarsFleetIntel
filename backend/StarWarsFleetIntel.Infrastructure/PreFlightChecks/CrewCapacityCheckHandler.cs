using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Infrastructure.PreFlightChecks
{
    public class CrewCapacityCheckHandler : BasePreFlightCheckHandler
    {
        protected override Task<PreFlightCheckResult> CheckAsync(Starship starship)
        {
            var result = new PreFlightCheckResult { CheckName = "Crew Capacity Check", IsPassed = true };

            if (string.IsNullOrEmpty(starship.Crew) || starship.Crew == "unknown")
            {
                result.Warnings.Add("Crew capacity is unknown");
            }
            else if (starship.Crew == "0")
            {
                result.Errors.Add("No crew capacity - autonomous operation required");
                result.IsPassed = false;
            }

            return Task.FromResult(result);
        }
    }
}
