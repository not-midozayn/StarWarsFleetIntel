using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFleetIntel.Application.Interfaces;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Infrastructure.Decorators
{
    public class ShieldEnhancementDecorator : IStarshipDecorator
    {
        public Starship Decorate(Starship starship)
        {
            starship.SpecialModifications.Add("Shield Enhancement");
            return starship;
        }
    }
}
