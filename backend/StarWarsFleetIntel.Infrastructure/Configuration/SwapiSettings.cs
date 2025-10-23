using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFleetIntel.Infrastructure.Configuration
{
    public class SwapiSettings
    {
        public const string SectionName = "SwapiSettings";

        public string BaseUrl { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
        public int RetryCount { get; set; } = 3;
        public int CircuitBreakerFailureThreshold { get; set; } = 5;
        public int CircuitBreakerDurationSeconds { get; set; } = 30;
    }
}
