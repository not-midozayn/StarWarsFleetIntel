using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Application.Interfaces
{
    public interface IStarshipDecorator
    {
        Starship Decorate(Starship starship);
    }
}
