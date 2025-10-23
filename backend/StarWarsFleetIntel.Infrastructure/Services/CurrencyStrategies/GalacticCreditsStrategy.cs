using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFleetIntel.Infrastructure.Services.CurrencyStrategies
{
    public class GalacticCreditsStrategy : ICurrencyConversionStrategy
    {
        private readonly decimal _conversionRate = 1.0m;
        public decimal Convert(decimal amountInCredits) => amountInCredits * _conversionRate;

        public string GetSymbol() => "GCC";

    }
}
