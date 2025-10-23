using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFleetIntel.Domain.Enums;

namespace StarWarsFleetIntel.Application.Interfaces
{
    public interface ICurrencyConverter
    {
        decimal Convert(decimal amountInCredits, Currency targetCurrency);
        string GetCurrencySymbol(Currency currency);
    }
}
