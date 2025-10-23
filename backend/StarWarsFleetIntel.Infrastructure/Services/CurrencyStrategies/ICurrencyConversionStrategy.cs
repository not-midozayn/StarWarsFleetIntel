using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFleetIntel.Infrastructure.Services.CurrencyStrategies
{
    public interface ICurrencyConversionStrategy
    {
        decimal Convert(decimal amountInCredits);
        string GetSymbol();
    }
}
