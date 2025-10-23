
using StarWarsFleetIntel.Domain.Enums;

namespace StarWarsFleetIntel.Application.Interfaces
{
    public interface IStarshipDecoratorFactory
    {
        IStarshipDecorator CreateDecorator(ModificationType modificationType);
    }
}
