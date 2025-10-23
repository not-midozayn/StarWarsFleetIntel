
using StarWarsFleetIntel.Application.Interfaces;
using StarWarsFleetIntel.Domain.Enums;
using StarWarsFleetIntel.Infrastructure.Services.CurrencyStrategies;

namespace StarWarsFleetIntel.Infrastructure.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly Dictionary<Currency, ICurrencyConversionStrategy> _strategies;

        public CurrencyConverter()
        {
            _strategies = new Dictionary<Currency, ICurrencyConversionStrategy>
            {
                { Currency.GalacticCredits, new GalacticCreditsStrategy() },
                { Currency.ImperialCredits, new ImperialCreditsStrategy() },
                { Currency.Wupiupi, new WupiupiCreditsStrategy() },
                { Currency.Truguts, new TrugutsCreditsStrategy() },
                { Currency.Peggats, new PeggatsCreditsStrategy() }
            };
        }

        public decimal Convert(decimal amountInCredits, Currency targetCurrency)
        {
            if (!_strategies.TryGetValue(targetCurrency, out var strategy))
            {
                throw new NotSupportedException($"Currency {targetCurrency} is not supported");
            }

            return strategy.Convert(amountInCredits);
        }

        public string GetCurrencySymbol(Currency currency)
        {
            if (!_strategies.TryGetValue(currency, out var strategy))
            {
                return "?";
            }

            return strategy.GetSymbol();
        }
    }
}