using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Application.Interfaces
{
    public interface IPreFlightCheckHandler
    {
        IPreFlightCheckHandler SetNext(IPreFlightCheckHandler handler);
        Task<PreFlightCheckResult> HandleAsync(Starship starship);
    }
}
