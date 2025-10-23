using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFleetIntel.Domain.Entities
{
    public class PreFlightCheckResult
    {
        public bool IsPassed { get; set; }
        public List<string> Warnings { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public string CheckName { get; set; } = string.Empty;
    }
}
