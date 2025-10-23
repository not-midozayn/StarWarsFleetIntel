using StarWarsFleetIntel.Application.Interfaces;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Infrastructure.Decorators
{
    public class EngineBoostDecorator : IStarshipDecorator
    {
        public Starship Decorate(Starship starship)
        {
            starship.SpecialModifications.Add("Engine Boost");
            return starship;
        }
    }
}