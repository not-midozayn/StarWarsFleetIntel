using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFleetIntel.Infrastructure.Services.CurrencyStrategies
{
    public class WupiupiCreditsStrategy : ICurrencyConversionStrategy
    {
        private const decimal ConversionRate = 0.625m;
        public decimal Convert(decimal amountInCredits) => amountInCredits * ConversionRate;
        public string GetSymbol() => "₩UP";
    }
}
