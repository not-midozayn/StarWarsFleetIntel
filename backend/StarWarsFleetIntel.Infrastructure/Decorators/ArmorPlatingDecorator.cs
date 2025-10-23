using StarWarsFleetIntel.Application.Interfaces;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Infrastructure.Decorators
{
    public class ArmorPlatingDecorator : IStarshipDecorator
    {
        public Starship Decorate(Starship starship)
        {
            starship.SpecialModifications.Add("Armor Plating");
            return starship;
        }
    }
}