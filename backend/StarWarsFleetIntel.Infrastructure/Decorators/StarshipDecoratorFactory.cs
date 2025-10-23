using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFleetIntel.Application.Interfaces;
using StarWarsFleetIntel.Domain.Enums;

namespace StarWarsFleetIntel.Infrastructure.Decorators
{
    public class StarshipDecoratorFactory : IStarshipDecoratorFactory
    {
        public IStarshipDecorator CreateDecorator(ModificationType modificationType)
        {
            return modificationType switch
            {
                ModificationType.WeaponUpgrade => new WeaponUpgradeDecorator(),
                ModificationType.ShieldEnhancement => new ShieldEnhancementDecorator(),
                ModificationType.EngineBoost => new EngineBoostDecorator(),
                ModificationType.ArmorPlating => new ArmorPlatingDecorator(),
                ModificationType.StealthSystem => new StealthSystemDecorator(),
                _ => throw new ArgumentException($"Unknown modification type: {modificationType}")
            };
        }
    }
}
