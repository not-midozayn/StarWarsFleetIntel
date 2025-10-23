using StarWarsFleetIntel.Application.Interfaces;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Infrastructure.Decorators
{
    public class StealthSystemDecorator : IStarshipDecorator
    {
        public Starship Decorate(Starship starship)
        {
            starship.SpecialModifications.Add("Stealth System");
            return starship;
        }
    }
}