using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFleetIntel.Infrastructure.Services.CurrencyStrategies
{
    public class ImperialCreditsStrategy : ICurrencyConversionStrategy
    {
        private const decimal ConversionRate = 1.2m; 
        public decimal Convert(decimal amountInCredits) => amountInCredits * ConversionRate;

        public string GetSymbol() => "IMP";
    }
}
