using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFleetIntel.Domain.Entities
{
    public class Starship
    {
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string CostInCredits { get; set; } = string.Empty;
        public string Length { get; set; } = string.Empty;
        public string MaxAtmospheringSpeed { get; set; } = string.Empty;
        public string Crew { get; set; } = string.Empty;
        public string Passengers { get; set; } = string.Empty;
        public string CargoCapacity { get; set; } = string.Empty;
        public string Consumables { get; set; } = string.Empty;
        public string HyperdriveRating { get; set; } = string.Empty;
        public string MGLT { get; set; } = string.Empty;
        public string StarshipClass { get; set; } = string.Empty;
        public List<string> Pilots { get; set; } = new();
        public List<string> Films { get; set; } = new();
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string Url { get; set; } = string.Empty;

        public List<string> SpecialModifications { get; set; } = new();
        public decimal? CostInCreditsNumeric =>
            decimal.TryParse(CostInCredits, out var cost) ? cost : null;
    }
}
