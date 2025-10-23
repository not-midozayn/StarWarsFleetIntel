using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFleetIntel.Infrastructure.Services.CurrencyStrategies
{
    public class TrugutsCreditsStrategy : ICurrencyConversionStrategy
    {
        private readonly decimal ConversionRate = 2m;
        public decimal Convert(decimal amountInCredits) => amountInCredits * ConversionRate;

        public string GetSymbol() => "TRU";
    }
}
