using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Infrastructure.PreFlightChecks
{
    public class ConsumablesCheckHandler : BasePreFlightCheckHandler
    {
        protected override Task<PreFlightCheckResult> CheckAsync(Starship starship)
        {
            var result = new PreFlightCheckResult { CheckName = "Consumables Check", IsPassed = true };

            if (string.IsNullOrEmpty(starship.Consumables) || starship.Consumables == "unknown")
            {
                result.Warnings.Add("Consumables duration unknown - recommend supply check");
            }

            return Task.FromResult(result);
        }
    }
}
